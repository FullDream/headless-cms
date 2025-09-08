import { ChangeDetectionStrategy, Component, input } from '@angular/core'
import { TableModule } from 'primeng/table'

export type ColumnDef = {
	field: string
	header?: string
}

@Component({
	selector: 'ui-dynamic-table',
	imports: [TableModule],
	templateUrl: './dynamic-table.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicTable<T> {
	readonly columnDefs = input<ColumnDef[]>([])

	readonly data = input.required<T[]>()
}
