import { inject, Service } from '@angular/core'
import { HttpClient, HttpErrorResponse } from '@angular/common/http'
import { CreateQueryOptions } from '@tanstack/angular-query-experimental'
import { type AccessResourceDto, ProblemDetails, ValidationProblemDetails } from './generated/models'
import { getAccessResourcesQueryOptions } from './generated'

type AccessResourcesError = HttpErrorResponse & {
	error: ValidationProblemDetails | ProblemDetails
}

@Service()
export class AccessResourcesQueryOptions {
	readonly #http = inject(HttpClient)

	getAccessResources(): CreateQueryOptions<AccessResourceDto[], AccessResourcesError> {
		return getAccessResourcesQueryOptions<AccessResourceDto[], AccessResourcesError>(this.#http, {
			query: { staleTime: 5 * 60_000 },
		})
	}
}
