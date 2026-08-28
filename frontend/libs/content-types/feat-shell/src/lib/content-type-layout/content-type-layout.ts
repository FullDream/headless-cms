import { Component, computed, inject } from '@angular/core'
import { SecondarySidebarPortal } from '@headless-cms/shared/ui'
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router'
import { PIcon } from '@primeicons/angular'
import {
	SidebarContent,
	SidebarGroup,
	SidebarGroupContent,
	SidebarGroupLabel,
	SidebarHeader,
	SidebarMenu,
	SidebarMenuButton,
	SidebarMenuItem,
} from 'primeng/sidebar'
import { ContentTypeQueryOptions } from '@headless-cms/content-types/data-access'
import { injectQuery } from '@tanstack/angular-query-experimental'
import { groupByProp, pipe } from 'remeda'
import { ButtonDirective } from 'primeng/button'
import { Tooltip } from 'primeng/tooltip'
import { Skeleton } from 'primeng/skeleton'
import { Message } from 'primeng/message'

@Component({
	selector: 'ct-feat-layout',
	imports: [
		SecondarySidebarPortal,
		RouterOutlet,
		PIcon,
		SidebarGroup,
		SidebarGroupContent,
		SidebarGroupLabel,
		SidebarMenu,
		SidebarMenuButton,
		SidebarMenuItem,
		RouterLink,
		RouterLinkActive,
		ButtonDirective,
		SidebarContent,
		SidebarHeader,
		Tooltip,
		Skeleton,
		Message,
	],
	templateUrl: './content-type-layout.html',
})
export class ContentTypeLayout {
	protected readonly queryList = injectQuery(() => this.#queryOptions.list)

	readonly #queryOptions = inject(ContentTypeQueryOptions)

	protected readonly skeletons = { collection: Array.from({ length: 10 }), singleton: Array.from({ length: 4 }) }

	protected readonly menu = computed(() =>
		pipe(
			this.queryList.data() || [],
			groupByProp('kind'),
			grouped =>
				[
					{
						kind: 'collection',
						label: 'Collection Types',
						emptyLabel: 'No collection types yet',
						items: grouped.collection,
					},
					{
						kind: 'singleton',
						label: 'Single Types',
						emptyLabel: 'No single types yet',
						items: grouped.singleton,
					},
				] as const,
		),
	)
}
