import { Directive, inject } from '@angular/core'
import { PageHeaderService } from './page-header.service'

@Directive({
	selector: '[uiPageHeaderOutlet]',
	exportAs: 'uiPageHeaderOutlet',
	providers: [PageHeaderService],
})
export class PageHeaderOutlet {
	readonly #service = inject(PageHeaderService)

	readonly portal = this.#service.portal
	readonly breadcrumbs = this.#service.breadcrumbs
	readonly loading = this.#service.loading
}
