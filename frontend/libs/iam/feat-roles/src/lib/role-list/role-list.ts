import { Component, inject } from '@angular/core'
import { RouterLink } from '@angular/router'
import { PageHeaderPortal } from '@headless-cms/shared/ui'
import { ButtonDirective } from 'primeng/button'
import { Plus, ChevronRight, EllipsisV } from '@primeicons/angular'
import { ColumnDef, DynamicTable, DynamicTableCell } from '@headless-cms/shared/ui-dynamic-table'
import { RolesQueryOptions } from '@headless-cms/iam/data-access'
import { Tag } from 'primeng/tag'
import { Tooltip } from 'primeng/tooltip'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'

@Component({
	selector: 'iam-role-list',
	imports: [
		PageHeaderPortal,
		ButtonDirective,
		RouterLink,
		DynamicTable,
		DynamicTableCell,
		Tag,
		Plus,
		ChevronRight,
		EllipsisV,
		Tooltip,
	],
	templateUrl: './role-list.html',
})
export class RoleList {
	protected readonly columns: ColumnDef[] = [
		{ header: 'Name', field: 'name', skeleton: true, skeletonClass: 'w-24', cellClass: 'h-14' },
		{ header: '', field: 'kind', skeleton: false, cellClass: 'h-14' },
		{
			header: '',
			field: 'actions',
			skeleton: false,
			columnClass: 'w-30',
			cellClass: 'flex gap-2 h-14 justify-end',
		},
	]

	protected readonly roles = injectQuery(() => this.#rolesQueryOptions.getList())
	protected readonly removeRole = injectMutation(() => this.#rolesQueryOptions.remove())

	readonly #rolesQueryOptions = inject(RolesQueryOptions)

	constructor() {}
}
