import { Component, computed, inject, input } from '@angular/core'

import { ColumnDef, DynamicTable } from '@headless-cms/shared/ui-dynamic-table'
import { injectQuery } from '@tanstack/angular-query-experimental'
import { ContentEntriesQueryOptions } from '@headless-cms/content-entries/data-access'
import { ContentSchema, ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { ButtonDirective, ButtonIcon, ButtonLabel } from 'primeng/button'
import { RouterLink } from '@angular/router'
import { PrimeIcons } from 'primeng/api'

const mapSchemaToColumnDefs = (schema?: ContentSchema): ColumnDef[] =>
	(schema?.fields ?? []).map(field => ({
		header: field.label,
		field: field.name,
	}))
@Component({
	selector: 'ce-feat-list',
	imports: [DynamicTable, RouterLink, ButtonDirective, ButtonLabel, ButtonIcon],
	templateUrl: './content-entries-feat-list.html',
})
export class ContentEntriesFeatList {
	readonly typeName = input.required<string>()

	protected readonly columnDefs = computed(() => mapSchemaToColumnDefs(this.#schemaQuery.data()))

	#schema = inject(ContentSchemaProvider)

	#schemaQuery = injectQuery(() => this.#schema.getSchemaOptions(this.typeName()))
	#queryOptions = inject(ContentEntriesQueryOptions)

	protected readonly listQuery = injectQuery(() => this.#queryOptions.getList(this.typeName()))
	protected readonly actions = [{ icon: PrimeIcons.PENCIL, routerLink: '6f7abad4-ac1a-436b-8268-7d9329a99f0e' }]
}
