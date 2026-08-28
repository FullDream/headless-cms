import { Directive, inject } from '@angular/core'
import { SecondarySidebarService } from './secondary-sidebar.service'

@Directive({
	selector: '[uiSecondarySidebar]',
	exportAs: 'uiSecondarySidebar',
	providers: [SecondarySidebarService],
})
export class SecondarySidebar {
	readonly #service = inject(SecondarySidebarService)

	readonly portal = this.#service.portal
}
