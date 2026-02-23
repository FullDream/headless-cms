import { ChangeDetectionStrategy, Component, input } from '@angular/core'
import { TableModule } from 'primeng/table'
import { Button, ButtonIcon } from 'primeng/button'
import { RouterLink } from '@angular/router'

export type ColumnDef = {
	field: string
	header?: string
}

export type ColumnAction<T> = {
	icon: string
	routerLink?: RouterLink['routerLink']
	command?: (item: T) => void
}

@Component({
	selector: 'ui-dynamic-table',
	imports: [TableModule, Button, RouterLink, ButtonIcon],
	templateUrl: './dynamic-table.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicTable<T> {
	readonly columnDefs = input<ColumnDef[]>([])
	readonly actions = input<ColumnAction<T>[]>()

	readonly data = input.required<T[]>()
}
