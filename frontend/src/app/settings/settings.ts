import { ChangeDetectionStrategy, Component } from '@angular/core'
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router'
import { SecondarySidebarPortal } from '@headless-cms/shared/ui'
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
	selector: 'app-settings',
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
	templateUrl: './settings.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Settings {}
