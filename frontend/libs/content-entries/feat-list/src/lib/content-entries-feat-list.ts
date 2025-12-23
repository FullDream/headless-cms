import { Component, computed, inject, input } from '@angular/core'

import { ColumnDef, DynamicTable } from '@headless-cms/shared/ui-dynamic-table'
import { injectQuery } from '@tanstack/angular-query-experimental'
import { ContentEntriesQueryOptions } from '@headless-cms/content-entries/data-access'
import { ContentSchema, ContentSchemaProvider } from '@headless-cms/shared/data-access'

const mapSchemaToColumnDefs = (schema?: ContentSchema): ColumnDef[] =>
	(schema?.fields ?? []).map(field => ({
		header: field.label,
		field: field.name,
	}))
@Component({
	selector: 'ce-feat-list',
	imports: [DynamicTable],
	templateUrl: './content-entries-feat-list.html',
})
export class ContentEntriesFeatList {
	readonly typeName = input.required<string>()

	protected readonly columnDefs = computed(() => mapSchemaToColumnDefs(this.#schemaQuery.data()))

	#schema = inject(ContentSchemaProvider)

	#schemaQuery = injectQuery(() => this.#schema.getSchemaOptions(this.typeName()))
	#queryOptions = inject(ContentEntriesQueryOptions)

	protected readonly listQuery = injectQuery(() => this.#queryOptions.getList(this.typeName()))
}
