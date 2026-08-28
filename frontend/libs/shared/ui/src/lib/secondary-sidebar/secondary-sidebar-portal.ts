import { CdkPortal } from '@angular/cdk/portal'
import { DestroyRef, Directive, inject, input } from '@angular/core'
import { SecondarySidebarService } from './secondary-sidebar.service'

@Directive({ selector: 'ng-template[uiSecondarySidebarPortal]', hostDirectives: [{ directive: CdkPortal }] })
export class SecondarySidebarPortal {
	readonly sidebarTitle = input('')

	readonly #service = inject(SecondarySidebarService)
	readonly #portal = inject(CdkPortal, { host: true, self: true })

	constructor() {
		this.#service.attach(this.#portal)

		inject(DestroyRef).onDestroy(() => this.#service.detach(this.#portal))
	}
}
