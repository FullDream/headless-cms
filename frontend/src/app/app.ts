import { Component, inject } from '@angular/core'
import { RouterModule } from '@angular/router'
import { ContentTypeDataSource } from '@headless-cms/content-types-data-access'

@Component({
	imports: [RouterModule],
	selector: 'app-root',
	templateUrl: './app.html',
})
export class App {
	protected title = 'headless-cms'

	#data = inject(ContentTypeDataSource)

	items = this.#data.list.data
}
