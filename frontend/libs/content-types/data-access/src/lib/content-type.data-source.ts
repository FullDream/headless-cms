import { inject, Injectable } from '@angular/core'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { HttpClient } from '@angular/common/http'
import { lastValueFrom } from 'rxjs'
import { components } from '@headless-cms/shared-util-api-types'

type ContentType = components['schemas']['ContentTypeDto']
type CreateContentType = components['schemas']['CreateContentTypeCommand']
type UpdateContentType = components['schemas']['UpdateContentTypeDto']

@Injectable({ providedIn: 'root' })
export class ContentTypeDataSource {
	#client = inject(HttpClient)

	readonly list = injectQuery(() => ({
		queryKey: ['content-types'],
		queryFn: () => lastValueFrom(this.#client.get<ContentType[]>('/api/content-types')),
	}))

	readonly create = injectMutation(() => ({
		mutationFn: (item: CreateContentType) =>
			lastValueFrom(this.#client.post<ContentType>('/api/content-types', item)),
	}))

	readonly update = injectMutation(() => ({
		mutationFn: ({ id, item }: { id: string; item: UpdateContentType }) =>
			lastValueFrom(this.#client.patch<ContentType>(`/api/content-types/${id}`, item)),
	}))

	readonly delete = injectMutation(() => ({
		mutationFn: (id: string) =>
			lastValueFrom(this.#client.delete<ContentType[]>(`/api/content-types${id}`)),
	}))
}
