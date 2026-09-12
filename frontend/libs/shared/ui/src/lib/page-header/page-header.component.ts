import { CdkPortalOutlet, Portal } from '@angular/cdk/portal'
import { Component, computed, input } from '@angular/core'
import { RouterLink } from '@angular/router'
import { MenuItem } from 'primeng/api'
import { Breadcrumb } from 'primeng/breadcrumb'
import { PageHeaderBreadcrumb } from './page-header'
import { ProgressBar } from 'primeng/progressbar'
import { toObservable, toSignal } from '@angular/core/rxjs-interop'
import { map, of, timer, switchMap } from 'rxjs'

@Component({
	selector: 'ui-page-header',
	imports: [Breadcrumb, RouterLink, CdkPortalOutlet, ProgressBar],
	templateUrl: './page-header.html',
})
export class PageHeaderComponent {
	readonly breadcrumbs = input<readonly PageHeaderBreadcrumb[]>([])
	readonly portal = input<Portal<unknown> | null>(null)
	readonly loading = input(false)

	protected readonly breadcrumbItems = computed<MenuItem[]>(() =>
		this.breadcrumbs().map(({ label, routerLink }) => ({ label, routerLink })),
	)

	protected readonly showLoading = toSignal(
		toObservable(this.loading).pipe(switchMap(value => (value ? timer(150).pipe(map(() => true)) : of(false)))),
		{ initialValue: false },
	)
}
