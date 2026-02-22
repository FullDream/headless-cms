import { Component, computed, inject } from '@angular/core'
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router'
import { ContentTypeQueryOptions } from '@headless-cms/content-types/data-access'
import { injectMutation, injectQuery, QueryClient } from '@tanstack/angular-query-experimental'
import { Button, ButtonDirective } from 'primeng/button'
import { AuthOptions } from '@headless-cms/iam/data-access'
import { MenuItem, MenuItemCommandEvent, PrimeIcons } from 'primeng/api'
import { Menu } from 'primeng/menu'
import { injectLocalStorage } from 'ngxtension/inject-local-storage'

export type NavItem = {
	label: string
	id: string
	icon?: string
	routerLink?: string
	items?: NavItem[]
}

type Theme = 'auto' | 'light' | 'dark'

const themeIconMap: Record<Theme, string> = {
	auto: PrimeIcons.DESKTOP,
	dark: PrimeIcons.MOON,
	light: PrimeIcons.SUN,
}

@Component({
	selector: 'app-layout',
	templateUrl: './layout.html',
	imports: [ButtonDirective, RouterOutlet, RouterLink, RouterLinkActive, Menu, Button],
})
export class LayoutComponent {
	readonly #authOptions = inject(AuthOptions)
	readonly #queryOptions = inject(ContentTypeQueryOptions)
	readonly queryClient = inject(QueryClient)

	readonly theme = injectLocalStorage<'auto' | 'light' | 'dark'>('theme', { defaultValue: 'auto' })
	readonly router = inject(Router)

	protected readonly queryList = injectQuery(() => this.#queryOptions.list)

	protected readonly logoutMutation = injectMutation(() => this.#authOptions.logout())
	protected readonly currentThemeIcon = computed(() => themeIconMap[this.theme()])

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

	protected readonly themeMenu = computed<MenuItem[]>(() => [
		{
			label: 'Light',
			icon: 'pi pi-sun',
			value: 'light_mode',
			disabled: this.theme() === 'light',
			command: (event: MenuItemCommandEvent) => this.theme.set('light'),
		},
		{
			label: 'Auto',
			icon: 'pi pi-desktop',
			value: 'auto',
			disabled: this.theme() === 'auto',
			command: () => this.theme.set('auto'),
		},
		{
			label: 'Dark',
			icon: 'pi pi-moon',
			value: 'dark_mode',
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
