import { inject, Injectable } from '@angular/core'
import { mutationOptions, QueryClient, queryOptions } from '@tanstack/angular-query-experimental'
import { HttpClient } from '@angular/common/http'
import { lastValueFrom } from 'rxjs'
import * as R from 'remeda'
import {
	ContentFieldDto,
	ContentTypeDto,
	CreateContentFieldDto,
	CreateContentTypeDto,
	DeleteContentTypesByIdErrors,
	DeleteContentTypesByIdFieldsByFieldIdErrors,
	DeleteContentTypesByIdFieldsByFieldIdResponse,
	DeleteContentTypesByIdResponse,
	GetContentTypesByIdErrors,
	GetContentTypesByIdResponse,
	GetContentTypesByNameResponse,
	GetContentTypesErrors,
	GetContentTypesResponse,
	PatchContentTypesByIdErrors,
	PatchContentTypesByIdFieldsByFieldIdResponse,
	PatchContentTypesByIdResponse,
	PostContentTypesByIdFieldsErrors,
	PostContentTypesByIdFieldsResponse,
	PostContentTypesErrors,
	PostContentTypesResponse,
	UpdateContentFieldDto,
	UpdateContentTypeDto,
} from './generated/types.gen'
import { ApiErrorResponse } from '@headless-cms/shared/util-http'
import { CreateQueryOptionsWithRealtime } from '@headless-cms/shared/data-access'

export type UpdateFieldVariables = { contentTypeId: string; fieldId: string; dto: UpdateContentFieldDto }
export type CreateFieldVariables = { contentTypeId: string; dto: CreateContentFieldDto }
export type DeleteFieldVariables = { contentTypeId: string; fieldId: string }
export type UpdateContentTypeVariables = { id: string; dto: UpdateContentTypeDto }
@Injectable({ providedIn: 'root' })
export class ContentTypeQueryOptions {
	readonly #client = inject(HttpClient)
	readonly #contentTypeKey = 'content-types'
	readonly #contentTypesListKey = [this.#contentTypeKey, 'list']
	readonly #contentTypesDetailKey = [this.#contentTypeKey, 'detail']
	readonly #apiUrl = `/api/${this.#contentTypeKey}`
	readonly #queryClient = inject(QueryClient)

	readonly list: CreateQueryOptionsWithRealtime<GetContentTypesResponse, ApiErrorResponse<GetContentTypesErrors>> = {
		...queryOptions<GetContentTypesResponse, ApiErrorResponse<GetContentTypesErrors>>({
			queryKey: this.#contentTypesListKey,
			placeholderData: [],
			queryFn: () => lastValueFrom(this.#client.get<GetContentTypesResponse>(this.#apiUrl)),
		}),
		connectionKey: this.#contentTypeKey,
		realtimeHandlers: {
			updated: (payload: ContentTypeDto) => {
				this.#queryClient.setQueryData<ContentTypeDto>([...this.#contentTypesDetailKey, payload.id], ct =>
					ct ? { ...ct, ...payload } : undefined,
				)
				this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
					list
						? R.pipe(
								list,
								R.map(ct => (ct.id === payload.id ? { ...ct, ...payload } : ct)),
								R.sortBy(R.prop('name')),
							)
						: undefined,
				)
			},
		},
	}

	readonly create = mutationOptions<ContentTypeDto, ApiErrorResponse<PostContentTypesErrors>, CreateContentTypeDto>({
		mutationKey: [this.#contentTypeKey, 'create'],
		mutationFn: item => lastValueFrom(this.#client.post<PostContentTypesResponse>(this.#apiUrl, item)),
		onSuccess: data =>
			this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
				R.pipe(list ?? [], R.concat([data]), R.sortBy(R.prop('name'))),
			),
	})

	readonly update = mutationOptions<
		PatchContentTypesByIdResponse,
		ApiErrorResponse<PatchContentTypesByIdErrors>,
		UpdateContentTypeVariables
	>({
		mutationKey: [this.#contentTypeKey, 'update'],
		mutationFn: ({ id, dto }) =>
			lastValueFrom(this.#client.patch<PatchContentTypesByIdResponse>(`${this.#apiUrl}/${id}`, dto)),
		// onSuccess: data => {
		// 	this.#queryClient.setQueryData<ContentTypeDto>([...this.#contentTypesDetailKey, data.id], ct =>
		// 		ct ? { ...ct, ...data } : undefined,
		// 	)
		// 	this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
		// 		list
		// 			? R.pipe(
		// 					list,
		// 					R.map(ct => (ct.id === data.id ? { ...ct, ...data } : ct)),
		// 					R.sortBy(R.prop('name')),
		// 				)
		// 			: undefined,
		// 	)
		// },
	})

	readonly delete = mutationOptions<
		DeleteContentTypesByIdResponse,
		ApiErrorResponse<DeleteContentTypesByIdErrors>,
		string
	>({
		mutationKey: [this.#contentTypeKey, 'delete'],
		mutationFn: id => lastValueFrom(this.#client.delete<DeleteContentTypesByIdResponse>(`${this.#apiUrl}/${id}`)),
		onSuccess: data => {
			this.#queryClient.removeQueries({ queryKey: [...this.#contentTypesDetailKey, data.id] })

			this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
				list?.filter(ct => ct.id !== data.id),
			)
		},
	})

	createFieldForContentType = mutationOptions<
		PostContentTypesByIdFieldsResponse,
		ApiErrorResponse<PostContentTypesByIdFieldsErrors>,
		CreateFieldVariables
	>({
		mutationFn: variables =>
			lastValueFrom(
				this.#client.post<PostContentTypesByIdFieldsResponse>(
					`${this.#apiUrl}/${variables.contentTypeId}/fields`,
					variables.dto,
				),
			),
		onSuccess: (data, variables) => {
			this.#queryClient.setQueryData<GetContentTypesByNameResponse>(
				[this.#contentTypeKey, 'detail', variables.contentTypeId],
				ct => (ct ? { ...ct, fields: [...ct.fields, data] } : undefined),
			)
		},
	})

	readonly updateFieldForContentType = mutationOptions<
		PatchContentTypesByIdFieldsByFieldIdResponse,
		ApiErrorResponse<DeleteContentTypesByIdFieldsByFieldIdErrors>,
		UpdateFieldVariables
	>({
		mutationFn: (variables: UpdateFieldVariables) =>
			lastValueFrom(
				this.#client.patch<ContentFieldDto>(
					`${this.#apiUrl}/${variables.contentTypeId}/fields/${variables.fieldId}`,
					variables.dto,
				),
			),
		onSuccess: (data, variables) => {
			this.#queryClient.setQueryData<GetContentTypesByNameResponse>(
				[this.#contentTypeKey, 'detail', variables.contentTypeId],
				ct => (ct ? { ...ct, fields: ct.fields.map(f => (f.id === data.id ? data : f)) } : undefined),
			)
		},
	})

	deleteField = mutationOptions<
		DeleteContentTypesByIdFieldsByFieldIdResponse,
		ApiErrorResponse<DeleteContentTypesByIdFieldsByFieldIdErrors>,
		DeleteFieldVariables
	>({
		mutationFn: variables =>
			lastValueFrom(
				this.#client.delete<DeleteContentTypesByIdFieldsByFieldIdResponse>(
					`${this.#apiUrl}/${variables.contentTypeId}/fields/${variables.fieldId}`,
				),
			),
		onSuccess: (data, variables) => {
			this.#queryClient.setQueryData<GetContentTypesByNameResponse>(
				[this.#contentTypeKey, 'detail', variables.contentTypeId],
				ct => (ct ? { ...ct, fields: ct.fields.filter(field => field.id !== data.id) } : undefined),
			)
		},
	})

	getById(
		id?: string | null,
	): CreateQueryOptionsWithRealtime<GetContentTypesByIdResponse, ApiErrorResponse<GetContentTypesByIdErrors>, any> {
		const cached = this.#queryClient
			.getQueryData<GetContentTypesResponse>(this.#contentTypesListKey)
			?.find(ct => ct.id === id)

		return {
			...queryOptions<GetContentTypesByIdResponse, ApiErrorResponse<GetContentTypesByIdErrors>>({
				queryKey: [...this.#contentTypesDetailKey, id] as const,
				queryFn: () => lastValueFrom(this.#client.get<ContentTypeDto>(`${this.#apiUrl}/${id}`)),
				enabled: !!id,
				initialData: cached,
				staleTime: 600000,
				initialDataUpdatedAt: this.#queryClient.getQueryState(this.#contentTypesListKey)?.dataUpdatedAt,
			}),
			connectionKey: this.#contentTypeKey,
			realtimeHandlers: {
				updated: (payload: ContentTypeDto) => {
					this.#queryClient.setQueryData<ContentTypeDto>([...this.#contentTypesDetailKey, payload.id], ct =>
						ct ? { ...ct, ...payload } : undefined,
					)

					console.log('updated', payload)
					this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
						list
							? R.pipe(
									list,
									R.map(ct => (ct.id === payload.id ? { ...ct, ...payload } : ct)),
									R.sortBy(R.prop('name')),
								)
							: undefined,
					)
				},
			},
		}
	}
}
