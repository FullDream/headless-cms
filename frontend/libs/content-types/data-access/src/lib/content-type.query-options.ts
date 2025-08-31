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
	DeleteContentTypesByIdResponse,
	GetContentTypesByNameResponse,
	GetContentTypesResponse,
	PatchContentTypesByIdResponse,
	PostContentTypesByIdFieldsResponse,
	PostContentTypesErrors,
	PostContentTypesResponse,
	UpdateContentFieldDto,
	UpdateContentTypeDto,
} from './generated/types.gen'
import { ApiErrorResponse } from '@headless-cms/shared/util-http'

export type UpdateFieldVariables = { contentTypeId: string; fieldId: string; dto: UpdateContentFieldDto }
export type CreateFieldVariables = { contentTypeId: string; dto: CreateContentFieldDto }
export type DeleteFieldVariables = { contentTypeId: string; fieldId: string }

@Injectable({ providedIn: 'root' })
export class ContentTypeQueryOptions {
	readonly #client = inject(HttpClient)
	readonly #contentTypeKey = 'content-types'
	readonly #contentTypesListKey = [this.#contentTypeKey, 'list']
	readonly #contentTypesDetailKey = [this.#contentTypeKey, 'detail']
	readonly #apiUrl = `/api/${this.#contentTypeKey}`
	readonly #queryClient = inject(QueryClient)

	readonly list = queryOptions({
		queryKey: this.#contentTypesListKey,
		placeholderData: [],
		queryFn: () => lastValueFrom(this.#client.get<GetContentTypesResponse>(this.#apiUrl)),
	})

	readonly create = mutationOptions<ContentTypeDto, ApiErrorResponse<PostContentTypesErrors>, CreateContentTypeDto>({
		mutationKey: [this.#contentTypeKey, 'create'],
		mutationFn: item => lastValueFrom(this.#client.post<PostContentTypesResponse>(this.#apiUrl, item)),
		onSuccess: data =>
			this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
				R.pipe(list ?? [], R.concat([data]), R.sortBy(R.prop('name'))),
			),
	})

	readonly update = mutationOptions({
		mutationKey: [this.#contentTypeKey, 'update'],
		mutationFn: ({ id, item }: { id: string; item: UpdateContentTypeDto }) =>
			lastValueFrom(this.#client.patch<PatchContentTypesByIdResponse>(`${this.#apiUrl}/${id}`, item)),
		onSuccess: data => {
			this.#queryClient.setQueryData<ContentTypeDto>([...this.#contentTypesDetailKey, data.id], ct =>
				ct ? { ...ct, ...data } : undefined,
			)
			this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
				list
					? R.pipe(
							list,
							R.map(ct => (ct.id === data.id ? { ...ct, ...data } : ct)),
							R.sortBy(R.prop('name')),
						)
					: undefined,
			)
		},
	})

	readonly delete = mutationOptions({
		mutationKey: [this.#contentTypeKey, 'delete'],
		mutationFn: (id: string) =>
			lastValueFrom(this.#client.delete<DeleteContentTypesByIdResponse>(`${this.#apiUrl}/${id}`)),

		onSuccess: data => {
			this.#queryClient.removeQueries({ queryKey: [...this.#contentTypesDetailKey, data.id] })

			this.#queryClient.setQueryData<ContentTypeDto[]>(this.#contentTypesListKey, list =>
				list?.filter(ct => ct.id !== data.id),
			)
		},
	})

	readonly updateFieldForContentType = mutationOptions({
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

	createFieldForContentType = mutationOptions({
		mutationFn: (variables: CreateFieldVariables) =>
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

	deleteField = mutationOptions({
		mutationFn: (variables: DeleteFieldVariables) =>
			lastValueFrom(
				this.#client.delete<ContentFieldDto>(
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

	getById(id?: string | null) {
		const cached = this.#queryClient
			.getQueryData<GetContentTypesResponse>(this.#contentTypesListKey)
			?.find(ct => ct.id === id)

		return queryOptions({
			queryKey: [...this.#contentTypesDetailKey, id],
			queryFn: () => lastValueFrom(this.#client.get<ContentTypeDto>(`${this.#apiUrl}/${id}`)),
			enabled: !!id && !cached,
			initialData: cached,
		})
	}
}
