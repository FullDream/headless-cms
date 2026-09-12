import { computed, inject, Service, signal } from '@angular/core'
import {
	GuardsCheckEnd,
	GuardsCheckStart,
	NavigationCancel,
	NavigationEnd,
	NavigationError,
	NavigationStart,
	ResolveEnd,
	ResolveStart,
	RouteConfigLoadEnd,
	RouteConfigLoadStart,
	Router,
} from '@angular/router'
import { takeUntilDestroyed } from '@angular/core/rxjs-interop'

@Service()
export class NavigationLoading {
	readonly #pending = signal(0)

	readonly loading = computed(() => this.#pending() > 0)

	constructor() {
		inject(Router)
			.events.pipe(takeUntilDestroyed())
			.subscribe(event => {
				if (
					event instanceof RouteConfigLoadStart ||
					event instanceof NavigationStart ||
					event instanceof ResolveStart
				) {
					this.#pending.update(value => value + 1)
				}

				if (
					event instanceof RouteConfigLoadEnd ||
					event instanceof NavigationEnd ||
					event instanceof ResolveEnd
				) {
					this.#pending.update(value => Math.max(0, value - 1))
				}

				if (event instanceof NavigationCancel || event instanceof NavigationError) {
					this.#pending.set(0)
				}
			})
	}
}
