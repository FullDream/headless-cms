import { Component, computed, DOCUMENT, effect, inject, signal } from '@angular/core'
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router'
import { injectMutation, QueryClient } from '@tanstack/angular-query-experimental'
import { AuthOptions } from '@headless-cms/iam/data-access'
import { MenuItem, MenuItemCommandEvent } from 'primeng/api'
import { Menu, MenuModule } from 'primeng/menu'
import { injectLocalStorage } from 'ngxtension/inject-local-storage'
import { SidebarModule } from 'primeng/sidebar'
import { AvatarModule } from 'primeng/avatar'
import { PIcon } from '@primeicons/angular/p-icon'
import { Tooltip } from 'primeng/tooltip'
import { Ripple } from 'primeng/ripple'
import { SecondarySidebar } from '@headless-cms/shared/ui'
import { CdkPortalOutlet } from '@angular/cdk/portal'

export type NavItem = {
	label: string
	id: string
	icon?: string
	routerLink?: string
	items?: NavItem[]
}

type Theme = 'system' | 'light' | 'dark'

const themeIconMap: Record<Theme, string> = {
	system: 'desktop',
	dark: 'moon',
	light: 'sun',
}

@Component({
	selector: 'app-layout',
	templateUrl: './layout.html',
	imports: [
		RouterOutlet,
		RouterLink,
		RouterLinkActive,
		Menu,
		RouterOutlet,
		SidebarModule,
		AvatarModule,
		PIcon,
		Tooltip,
		RouterLink,
		RouterLinkActive,
		MenuModule,
		Ripple,
		CdkPortalOutlet,
		SecondarySidebar,
	],
})
export class LayoutComponent {
	readonly #authOptions = inject(AuthOptions)
	readonly queryClient = inject(QueryClient)

	protected readonly theme = injectLocalStorage<'light' | 'system' | 'dark'>('theme', { defaultValue: 'system' })
	protected readonly router = inject(Router)

	protected readonly logoutMutation = injectMutation(() => this.#authOptions.logout())
	protected readonly currentThemeIcon = computed(() => themeIconMap[this.theme()])

	protected readonly menuItems = [
		{
			link: '/',
			icon: 'home',
			label: 'Home',
			routerLinkActiveOptions: { exact: true },
		},
		{
			link: 'content-entries',
			icon: 'file-edit',
			label: 'Content Manager',
		},
		{
			link: 'content-types',
			icon: 'database',
			label: 'Content Type Builder',
		},
	]

	userItems: MenuItem[] = [
		{
			label: 'john@acme.com',
			items: [
				{ label: 'Settings', icon: 'cog' },
				{ label: 'Notifications', icon: 'bell' },
				{ separator: true },
				{ label: 'Sign out', icon: 'sign-out', command: () => this.logout() },
			],
		},
	]
	protected readonly sidebarOpen = signal(false)

	constructor() {
		const document = inject(DOCUMENT)

		effect(() => {
			document.documentElement.dataset.theme = this.theme()
		})
	}

	protected toggleSidebar(): void {
		this.sidebarOpen.update(prev => !prev)
	}

	protected readonly themeMenu = computed<MenuItem[]>(() => [
		{
			label: 'Light',
			icon: 'sun',
			disabled: this.theme() === 'light',
			command: (event: MenuItemCommandEvent) => this.theme.set('light'),
		},
		{
			label: 'System',
			icon: 'desktop',
			disabled: this.theme() === 'system',
			command: () => this.theme.set('system'),
		},
		{
			label: 'Dark',
			icon: 'moon',
			disabled: this.theme() === 'dark',
			command: () => this.theme.set('dark'),
		},
	])
	protected logout(): void {
		this.logoutMutation.mutate(undefined, {
			onSuccess: () => {
				this.queryClient.clear()
				this.router.navigate(['/auth/login'], { replaceUrl: true })
			},
		})
	}
}
