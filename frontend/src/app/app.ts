import { Component, computed, inject } from '@angular/core'
import { RouterModule } from '@angular/router'
import { ContentTypeQueryOptions } from '@headless-cms/content-types/data-access'
import { FormsModule } from '@angular/forms'
import { injectQuery } from '@tanstack/angular-query-experimental'

export type NavItem = {
	label: string
	id: string
	icon?: string
	routerLink?: string
	items?: NavItem[]
}

@Component({
	imports: [RouterModule, FormsModule],
	selector: 'app-root',
	templateUrl: './app.html',
})
export class App {
	readonly #queryOptions = inject(ContentTypeQueryOptions)
	readonly queryList = injectQuery(() => this.#queryOptions.list)
	protected readonly menu = computed(() =>
		this.queryList.data()?.map(type => ({
			label: type.name,
			id: type.id,
			icon: `pi pi-${type.kind === 'collection' ? 'list' : 'file'}`,
			routerLink: `/content-types/${type.id}`,
		})),
	)

	protected readonly contentManagerMenu = computed(() =>
		this.queryList.data()?.map(type => ({
			label: type.name,
			id: type.id,
			icon: `pi pi-${type.kind === 'collection' ? 'list' : 'file'}`,
			routerLink: `/content-entries/${type.name}`,
		})),
	)

	constructor() {
		// const hub = inject(HubConnectionFactory).get<ContentTypesHub>('content-types')
		//
		// hub.connect().pipe(takeUntilDestroyed()).subscribe()
		//
		// hub.on<ContentTypeDto>('created')
		// 	.pipe(takeUntilDestroyed())
		// 	.subscribe(dto => {
		// 		console.log('created', dto)
		// 	})
		// hub.on<ContentTypeDto>('updated')
		// 	.pipe(takeUntilDestroyed())
		// 	.subscribe(dto => {
		// 		console.log('updated', dto)
		// 	})
		// hub.on<ContentTypeDto>('removed')
		// 	.pipe(takeUntilDestroyed())
		// 	.subscribe(dto => {
		// 		console.log('removed', dto)
		// 	})
	}
}
