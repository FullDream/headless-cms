import { Component, computed, inject } from '@angular/core'
import { RouterModule } from '@angular/router'
import { ContentTypeDataSource } from '@headless-cms/content-types-data-access'
import { PanelMenuModule } from 'primeng/panelmenu'
import { MenuItem } from 'primeng/api'

@Component({
	imports: [RouterModule, PanelMenuModule],
	selector: 'app-root',
	templateUrl: './app.html',
})
export class App {
	readonly #data = inject(ContentTypeDataSource)

	protected readonly menu = computed(() =>
		this.#buildMenu(
			this.#data.list.data().map(type => ({
				label: type.name,
				icon: `pi pi-${type.kind === 'collection' ? 'list' : 'file'}`,
				routerLink: `/content-types/${type.id}`,
			})),
		),
	)

	#buildMenu(items: MenuItem[]): MenuItem[] {
		return [
			{
				label: 'Home',
				icon: 'pi pi-home',
				routerLink: '/',
			},
			{
				label: 'Content Type Builder',
				icon: 'pi pi-database',
				items,
			},
		]
	}
}
