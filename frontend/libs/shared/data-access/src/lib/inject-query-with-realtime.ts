import { CreateQueryOptions, CreateQueryResult, injectQuery } from '@tanstack/angular-query-experimental'
import { DefaultError, QueryKey } from '@tanstack/query-core'
import { computed, DestroyRef, inject } from '@angular/core'
import { HubConnectionFactory } from '@ssv/signalr-client'
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop'
import { merge, switchMap, tap } from 'rxjs'
import * as R from 'remeda'

type RealtimeHandlers<TPayload> = Record<string, (payload: TPayload) => void>

export type CreateQueryOptionsWithRealtime<
	TQueryFnData = unknown,
	TError = DefaultError,
	TData = TQueryFnData,
	TRealtimeHandlers = RealtimeHandlers<any>,
	TQueryKey extends QueryKey = QueryKey,
> = CreateQueryOptions<TQueryFnData, TError, TData, TQueryKey> & {
	connectionKey: string
	realtimeHandlers: TRealtimeHandlers
}

export function injectQueryWithRealtime<
	TQueryFnData = unknown,
	TError = DefaultError,
	TData = TQueryFnData,
	TRealtimeHandlers extends RealtimeHandlers<any> = RealtimeHandlers<any>,
	TQueryKey extends QueryKey = QueryKey,
>(
	injectQueryFn: () => CreateQueryOptionsWithRealtime<TQueryFnData, TError, TData, TRealtimeHandlers, TQueryKey>,
): CreateQueryResult<TData, TError> {
	const options = injectQueryFn()
	const hub = inject(HubConnectionFactory)
		.create({ key: options.connectionKey, endpointUri: `/hubs/${options.connectionKey}` })
		.get<TRealtimeHandlers>(options.connectionKey)

	hub.connect().pipe(takeUntilDestroyed()).subscribe()

	const handlers = computed(() => {
		const options = injectQueryFn()

		return options.realtimeHandlers
	})

	toObservable(handlers)
		.pipe(
			switchMap(handlers =>
				merge(
					...R.pipe(
						handlers,
						R.entries(),
						R.map(([eventName, handler]) => hub.on<TData>(eventName).pipe(tap(handler))),
					),
				),
			),
			takeUntilDestroyed(),
		)
		.subscribe()

	inject(DestroyRef).onDestroy(() => hub.dispose())

	return injectQuery(injectQueryFn)
}
