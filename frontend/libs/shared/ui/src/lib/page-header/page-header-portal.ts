import { CdkPortal } from '@angular/cdk/portal'
import { booleanAttribute, DestroyRef, Directive, effect, inject, input, untracked } from '@angular/core'
import { PageHeaderBreadcrumb } from './page-header'
import { PageHeaderService } from './page-header.service'

@Directive({ selector: 'ng-template[uiPageHeaderPortal]', hostDirectives: [{ directive: CdkPortal }] })
export class PageHeaderPortal {
	readonly breadcrumbs = input<readonly PageHeaderBreadcrumb[]>([])
	readonly loading = input(false, { transform: booleanAttribute })

	readonly #service = inject(PageHeaderService)
	readonly #portal = inject(CdkPortal, { host: true, self: true })

	constructor() {
		this.#service.attach(this.#portal)

		effect(() => {
			const breadcrumbs = this.breadcrumbs()
			const loading = this.loading()

			untracked(() => this.#service.update(this.#portal, { breadcrumbs, loading }))
		})

		inject(DestroyRef).onDestroy(() => this.#service.detach(this.#portal))
	}
}
