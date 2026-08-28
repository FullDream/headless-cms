import { Component, computed, inject } from '@angular/core'
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { SecondarySidebarPortal } from '@headless-cms/shared/ui'
import { injectQuery } from '@tanstack/angular-query-experimental'
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

@Component({
	selector: 'ce-entry-layout',
	imports: [
		RouterOutlet,
		RouterLink,
		RouterLinkActive,
		SecondarySidebarPortal,
		SidebarContent,
		SidebarGroup,
		SidebarGroupContent,
		SidebarGroupLabel,
		SidebarHeader,
		SidebarMenu,
		SidebarMenuButton,
		SidebarMenuItem,
	],
	templateUrl: './content-entry-layout.html',
})
export class ContentEntryLayout {
	readonly #schema = inject(ContentSchemaProvider)

	protected readonly queryList = injectQuery(() => this.#schema.getSchemaListOptions())
	protected readonly collectionTypes = computed(
		() => this.queryList.data()?.filter(type => type.kind === 'collection') ?? [],
	)
	protected readonly singleTypes = computed(
		() => this.queryList.data()?.filter(type => type.kind === 'singleton') ?? [],
	)
}
