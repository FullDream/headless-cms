import { booleanAttribute, Component, computed, contentChildren, input } from '@angular/core'
import { TableModule } from 'primeng/table'
import { Button, ButtonDirective, ButtonIcon } from 'primeng/button'
import { RouterLink } from '@angular/router'
import { Skeleton } from 'primeng/skeleton'
import { DynamicTableCell } from './dynamic-table-cell'
import { NgTemplateOutlet } from '@angular/common'
import { Inbox } from '@primeicons/angular'

export type ColumnDef = {
	field: string
	header?: string
	columnClass?: string
	cellClass?: string
	skeleton?: boolean
	skeletonClass?: string
}

export type ColumnAction<T> = {
	icon: string
	routerLink?: RouterLink['routerLink']
	command?: (item: T) => void
}

@Component({
	selector: 'ui-dynamic-table',
	imports: [TableModule, Button, RouterLink, ButtonIcon, Skeleton, Inbox, NgTemplateOutlet, ButtonDirective],
	templateUrl: './dynamic-table.component.html',
})
export class DynamicTable<T> {
	readonly columnDefs = input<ColumnDef[]>([])
	readonly data = input.required<T[] | undefined>()
	readonly actions = input<ColumnAction<T>[]>()
	readonly loading = input(false, { transform: booleanAttribute })

	protected readonly placeholders = Array.from({ length: 6 }, () => ({}) as T)

	protected readonly cellTemplates = contentChildren<DynamicTableCell<T>>(DynamicTableCell)

	protected readonly cellTemplateMap = computed(
		() => new Map(this.cellTemplates()?.map(cell => [cell.column(), cell.templateRef])),
	)
}
