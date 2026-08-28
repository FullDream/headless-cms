import { Injectable, signal } from '@angular/core'
import type { Portal } from '@angular/cdk/portal'

@Injectable()
export class SecondarySidebarService {
	readonly #portal = signal<Portal<unknown> | null>(null)
	public readonly portal = this.#portal.asReadonly()

	attach(portal: Portal<unknown>): void {
		this.#portal.set(portal)
	}

	detach(portal: Portal<unknown>): void {
		if (this.#portal() === portal) {
			this.#portal.set(null)
		}
	}
}
