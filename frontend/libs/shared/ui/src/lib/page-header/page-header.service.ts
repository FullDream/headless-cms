import { Portal } from '@angular/cdk/portal'
import { computed, Injectable, signal } from '@angular/core'
import { PageHeaderBreadcrumb } from './page-header'

type PageHeaderState = {
	portal: Portal<unknown>
	breadcrumbs: readonly PageHeaderBreadcrumb[]
	loading: boolean
}

type PageHeaderStateUpdate = Partial<Pick<PageHeaderState, 'breadcrumbs' | 'loading'>>

@Injectable()
export class PageHeaderService {
	readonly #state = signal<PageHeaderState | null>(null)

	readonly portal = computed(() => this.#state()?.portal ?? null)
	readonly breadcrumbs = computed(() => this.#state()?.breadcrumbs ?? [])
	readonly loading = computed(() => this.#state()?.loading ?? false)

	attach(portal: Portal<unknown>, breadcrumbs: readonly PageHeaderBreadcrumb[] = [], loading = false): void {
		this.#state.set({
			portal,
			breadcrumbs,
			loading,
		})
	}

	update(portal: Portal<unknown>, update: PageHeaderStateUpdate): void {
		this.#state.update(state => (state?.portal === portal ? { ...state, ...update } : state))
	}

	detach(portal: Portal<unknown>): void {
		this.#state.update(state => (state?.portal === portal ? null : state))
	}
}
