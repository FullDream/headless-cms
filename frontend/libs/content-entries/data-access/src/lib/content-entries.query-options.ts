import { inject, Injectable } from '@angular/core'
import { MutationOptions, mutationOptions, queryOptions } from '@tanstack/angular-query-experimental'

import { ApiErrorResponse } from '@headless-cms/shared/util-http'
import { lastValueFrom } from 'rxjs'
import {
	GetContentEntriesByNameErrors,
	GetContentEntriesByNameResponse,
	PostContentEntriesByNameErrors,
	PostContentEntriesByNameResponse,
} from './generated/types.gen'
import { HttpClient } from '@angular/common/http'

type CreateEntryVariables = { typeName: string; dto: Record<string, unknown> }

@Injectable({ providedIn: 'root' })
export class ContentEntriesQueryOptions {
	readonly #contentEntriesKey = 'content-entries'
	readonly #httpClient = inject(HttpClient)

	getListKey(name: string): string[] {
		return [this.#contentEntriesKey, 'list', name] as const
	}

	getList(name: string) {
		return queryOptions<GetContentEntriesByNameResponse, ApiErrorResponse<GetContentEntriesByNameErrors>>({
			queryKey: this.getListKey(name),
			placeholderData: [],
			enabled: !!name,
			queryFn: () =>
				lastValueFrom(
					this.#httpClient.get<GetContentEntriesByNameResponse>(`api/${this.#contentEntriesKey}/${name}`),
				),
		})
	}

	create(): MutationOptions<
		PostContentEntriesByNameResponse,
		ApiErrorResponse<PostContentEntriesByNameErrors>,
		CreateEntryVariables
	> {
		return mutationOptions({
			mutationFn: variables =>
				lastValueFrom(
					this.#httpClient.post<PostContentEntriesByNameResponse>(
						`${this.#contentEntriesKey}/${variables.typeName}`,
						variables.dto,
					),
				),
		})
	}
}
