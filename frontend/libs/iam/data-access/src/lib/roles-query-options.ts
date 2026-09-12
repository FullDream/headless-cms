import { inject, Service } from '@angular/core'
import { HttpClient, HttpErrorResponse } from '@angular/common/http'
import { CreateMutationOptions, CreateQueryOptions, QueryClient } from '@tanstack/angular-query-experimental'
import { CreateRoleDto, ProblemDetails, type RoleDto, ValidationProblemDetails } from './generated/models'
import {
	CreateRoleMutationVariables,
	DeleteRoleMutationVariables,
	getCreateRoleMutationOptions,
	getDeleteRoleMutationOptions,
	getRoleByIdQueryKey,
	getRoleByIdQueryOptions,
	getRolesQueryKey,
	getRolesQueryOptions,
	getUpdateRoleMutationOptions,
	UpdateRoleMutationVariables,
} from './generated'
import { filter, map, pipe, prop, sortBy } from 'remeda'

type RolesError = HttpErrorResponse & {
	error: ValidationProblemDetails | ProblemDetails
}

type RemoveRoleOnMutateResult = {
	previousRoles: RoleDto[] | undefined
	previousRole: RoleDto | undefined
}

@Service()
export class RolesQueryOptions {
	readonly #http = inject(HttpClient)
	readonly #queryClient = inject(QueryClient)

	getList(): CreateQueryOptions<RoleDto[], RolesError> {
		return getRolesQueryOptions<RoleDto[], RolesError>(this.#http, { query: { staleTime: 5 * 60_000 } })
	}

	getById(id: string): CreateQueryOptions<RoleDto, RolesError> {
		return getRoleByIdQueryOptions<RoleDto, RolesError>(this.#http, id, {
			query: {
				initialData: () =>
					this.#queryClient.getQueryData<RoleDto[]>(getRolesQueryKey())?.find(role => role.id === id),
				initialDataUpdatedAt: () => this.#queryClient.getQueryState(getRolesQueryKey())?.dataUpdatedAt,
				staleTime: 5 * 60_000,
				enabled: !!id?.trim(),
			},
		})
	}

	create(): CreateMutationOptions<RoleDto, RolesError, CreateRoleDto> {
		const { mutationKey, mutationFn } = getCreateRoleMutationOptions<RolesError>(this.#http)

		return {
			mutationKey,
			mutationFn: (data, context) => mutationFn!({ data }, context),
			onSuccess: async (role, _, __, context) => {
				const rolesKey = getRolesQueryKey()
				const roleKey = getRoleByIdQueryKey(role.id)

				await context.client.cancelQueries({ queryKey: rolesKey })
				await context.client.cancelQueries({ queryKey: roleKey })

				context.client.setQueryData(roleKey, role)

				context.client.setQueryData<RoleDto[]>(rolesKey, roles =>
					roles ? pipe([...roles, role], sortBy([prop('kind'), 'desc'], prop('name'))) : undefined,
				)
			},
		}
	}

	update(): CreateMutationOptions<RoleDto, RolesError, UpdateRoleMutationVariables> {
		return getUpdateRoleMutationOptions<RolesError>(this.#http, {
			mutation: {
				onSuccess: async (role, _, __, context) => {
					const rolesKey = getRolesQueryKey()
					const roleKey = getRoleByIdQueryKey(role.id)

					await context.client.cancelQueries({ queryKey: rolesKey })
					await context.client.cancelQueries({ queryKey: roleKey })

					context.client.setQueryData(roleKey, role)

					context.client.setQueryData<RoleDto[]>(rolesKey, roles =>
						roles
							? pipe(
									roles,
									map(item => (item.id === role.id ? role : item)),
									sortBy([prop('kind'), 'desc'], prop('name')),
								)
							: undefined,
					)
				},
			},
		})
	}

	remove(): CreateMutationOptions<RoleDto, RolesError, string, RemoveRoleOnMutateResult> {
		const generatedOptions = getDeleteRoleMutationOptions<RolesError, RemoveRoleOnMutateResult>(this.#http)

		return {
			mutationKey: generatedOptions.mutationKey,

			mutationFn: (id, context) => generatedOptions.mutationFn!({ id }, context),

			onMutate: async (id, context) => {
				const rolesKey = getRolesQueryKey()
				const roleKey = getRoleByIdQueryKey(id)

				await context.client.cancelQueries({ queryKey: rolesKey })
				await context.client.cancelQueries({ queryKey: roleKey })

				const previousRoles = context.client.getQueryData<RoleDto[]>(rolesKey)

				const previousRole = context.client.getQueryData<RoleDto>(roleKey)

				context.client.setQueryData<RoleDto[]>(rolesKey, roles => roles?.filter(role => role.id !== id))

				context.client.removeQueries({ queryKey: roleKey })

				return { previousRoles, previousRole }
			},

			onError: (_error, id, result, context) => {
				if (result?.previousRoles) {
					context.client.setQueryData(getRolesQueryKey(), result.previousRoles)
				}

				if (result?.previousRole) {
					context.client.setQueryData(getRoleByIdQueryKey(id), result.previousRole)
				}
			},
		}
	}
}
