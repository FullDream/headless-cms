import { Component, computed, effect, inject, input, linkedSignal } from '@angular/core'
import { ButtonDirective } from 'primeng/button'
import { ErrorMessage, PageHeaderPortal } from '@headless-cms/shared/ui'
import { disabled, form, FormField, FormRoot, required } from '@angular/forms/signals'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { NgTemplateOutlet } from '@angular/common'
import { SelectButton } from 'primeng/selectbutton'
import {
	AccessResourcesQueryOptions,
	injectAccessResources,
	injectCreateRole,
	injectRoleById,
	RolesQueryOptions,
} from '@headless-cms/iam/data-access'
import {
	AccessResourceDto,
	AccessScope,
	PermissionAction,
	ResourceCapabilityDto,
	RolePermissionDto,
} from '../../../../data-access/src/lib/generated/models'
import { isNonNullish } from 'remeda'
import { HttpErrorResponse } from '@angular/common/http'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { ActivatedRoute, Router } from '@angular/router'

type RoleFormModel = {
	name: string
	resources: PermissionResourceModel[]
}

type PermissionResourceModel = {
	resourceId: string
	capabilities: PermissionCapabilityModel[]
	children: PermissionResourceModel[]
}

type PermissionCapabilityModel = {
	action: PermissionAction
	scope: AccessScope | null
}

type ScopeOptions = {
	label: string
	value: AccessScope | null
}

@Component({
	selector: 'iam-role-editor',
	imports: [
		ButtonDirective,
		PageHeaderPortal,
		FormField,
		FormRoot,
		FloatLabel,
		InputText,
		NgTemplateOutlet,
		SelectButton,
		ErrorMessage,
	],
	templateUrl: './role-editor.html',
})
export class RoleEditor {
	public readonly roleId = input<string, string>('', {
		transform: value => (value === 'new' ? '' : value),
	})

	readonly #roleOptions = inject(RolesQueryOptions)
	readonly #accessResourceOptions = inject(AccessResourcesQueryOptions)
	readonly #router = inject(Router)
	readonly #activatedRoute = inject(ActivatedRoute)

	protected roleById = injectQuery(() => this.#roleOptions.getById(this.roleId()))
	protected readonly resources = injectQuery(() => this.#accessResourceOptions.getAccessResources())
	protected readonly createRole = injectMutation(() => this.#roleOptions.create())
	protected readonly updateRole = injectMutation(() => this.#roleOptions.update())
	protected readonly isCreate = computed(() => !this.roleId())

	protected readonly model = linkedSignal(() => ({
		name: this.roleById.data()?.name || '',
		resources: this.#createPermissionResources(this.resources.data(), this.roleById.data()?.permissions),
	}))

	protected readonly roleForm = form(
		this.model,
		schemaPath => {
			required(schemaPath.name, { message: 'Role name is required' })

			disabled(schemaPath, {
				when: () => !this.canEdit(),
			})
		},
		{
			submission: {
				action: async form => {
					try {
						if (this.isCreate()) {
							const role = await this.createRole.mutateAsync({
								name: form.name().value(),
								permissions: this.#toPermissions(form.resources().value()),
							})

							form().reset()

							await this.#router.navigate(['../', role.id], {
								relativeTo: this.#activatedRoute,
								replaceUrl: true,
							})
						} else {
							await this.updateRole.mutateAsync({
								id: this.roleId(),
								data: {
									name: form.name().value(),
									permissions: this.#toPermissions(form.resources().value()),
								},
							})

							form().reset()
						}

						return null
					} catch (error) {
						console.log('err', (error as HttpErrorResponse).error.details)

						return {
							fieldTree: form.name,
							kind: 'server',
							message: (error as HttpErrorResponse).error.details,
						}
					}
				},
			},
		},
	)

	protected readonly nameErrors = computed(() =>
		this.roleForm
			.name()
			.errors()
			.map(error => error.message)
			.filter(isNonNullish),
	)

	protected readonly canEdit = computed(
		() =>
			this.resources.isSuccess() &&
			this.roleById.data()?.kind !== 'superAdmin' &&
			(!this.roleId() || this.roleById.isSuccess()),
	)

	protected readonly allScopeOptions: ScopeOptions[] = [
		{ label: 'None', value: null },
		{ label: 'Own', value: AccessScope.Own },
		{ label: 'All', value: AccessScope.All },
	]

	protected readonly allOnlyScopeOptions: ScopeOptions[] = [
		{ label: 'None', value: null },
		{ label: 'All', value: AccessScope.All },
	]

	protected scopeOptions(capability: ResourceCapabilityDto): ScopeOptions[] {
		return capability.availableScopes.includes(AccessScope.Own) ? this.allScopeOptions : this.allOnlyScopeOptions
	}

	#createPermissionResources(
		resources: AccessResourceDto[] = [],
		permissions: RolePermissionDto[] = [],
	): PermissionResourceModel[] {
		// @ts-ignore
		const permissionsByResource = Map.groupBy(permissions, permission => permission.resourceId)

		return this.#mapPermissionResources(resources, permissionsByResource)
	}

	#mapPermissionResources(
		resources: AccessResourceDto[],
		permissionsByResource: Map<string, RolePermissionDto[]>,
	): PermissionResourceModel[] {
		return resources.map(resource => {
			const permissions = permissionsByResource.get(resource.id) ?? []

			return {
				resourceId: resource.id,

				capabilities: resource.capabilities.map(capability => ({
					action: capability.action,
					scope: permissions.find(permission => permission.action === capability.action)?.scope ?? null,
				})),

				children: this.#mapPermissionResources(
					resource.children as unknown as AccessResourceDto[],
					permissionsByResource,
				),
			}
		})
	}

	#toPermissions(resources: PermissionResourceModel[]): RolePermissionDto[] {
		return resources.flatMap(resource => [
			...resource.capabilities.flatMap(capability =>
				capability.scope === null
					? []
					: [
							{
								resourceId: resource.resourceId,
								action: capability.action,
								scope: capability.scope,
							},
						],
			),
			...this.#toPermissions(resource.children),
		])
	}

	protected readonly form = form
}
