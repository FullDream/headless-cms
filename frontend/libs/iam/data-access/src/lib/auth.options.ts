import {
	CreateMutationOptions,
	CreateQueryOptions,
	mutationOptions,
	queryOptions,
} from '@tanstack/angular-query-experimental'
import { inject, Injectable } from '@angular/core'
import { lastValueFrom } from 'rxjs'
import { HttpClient, HttpErrorResponse } from '@angular/common/http'

type LoginVariables = {
	email: string
	password: string
}

@Injectable({ providedIn: 'root' })
export class AuthOptions {
	readonly #httpClient = inject(HttpClient)
	readonly #url = `api/iam/auth`

	public me(): CreateQueryOptions<void, HttpErrorResponse, void, ['iam', 'auth', 'me']> {
		return queryOptions({
			queryKey: ['iam', 'auth', 'me'],
			queryFn: () => lastValueFrom(this.#httpClient.get<void>(this.#url + '/me')),
		})
	}

	public login(): CreateMutationOptions<void, HttpErrorResponse, LoginVariables> {
		return mutationOptions({
			mutationKey: ['iam', 'auth', 'login'],
			mutationFn: variables => lastValueFrom(this.#httpClient.post<void>(this.#url + '/login', variables)),
			onSuccess: (data, _, __, { client }) => client.setQueryData(['iam', 'auth', 'me'], data),
		})
	}

	public logout(): CreateMutationOptions<void, HttpErrorResponse> {
		return mutationOptions({
			mutationKey: ['iam', 'auth', 'logout'],
			mutationFn: () => lastValueFrom(this.#httpClient.post<void>(this.#url + '/logout', null)),
		})
	}
}
