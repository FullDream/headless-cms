import { ChangeDetectionStrategy, Component, computed, inject, input } from '@angular/core'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { DynamicForm, DynamicFormConfig } from '@headless-cms/shared/ui-dynamic-form'
import { FieldTree } from '@angular/forms/signals'
import { ButtonDirective } from 'primeng/button'
import { ContentEntriesQueryOptions } from '@headless-cms/content-entries/data-access'

@Component({
	selector: 'ce-editor',
	imports: [DynamicForm, ButtonDirective],
	templateUrl: './content-entry-editor.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContentEntryEditor {
	readonly typeName = input.required<string>()

	readonly #schema = inject(ContentSchemaProvider)
	readonly contentEntryOptions = inject(ContentEntriesQueryOptions)

	readonly #schemaQuery = injectQuery(() => this.#schema.getSchemaOptions(this.typeName()))
	readonly createMutation = injectMutation(() => this.contentEntryOptions.create())

	protected readonly dynamicFormConfig = computed<DynamicFormConfig>(
		() =>
			this.#schemaQuery.data()?.fields.map(item => ({
				type: 'string',
				value: '',
				name: item.name,
				label: item.label ?? item.name,
			})) ?? [],
	)

	protected submit(event: Event, form: FieldTree<Record<string, string>, string | number>): void {
		event.preventDefault()


		this.createMutation.mutate({typeName: this.typeName(), dto: form().value()})
		console.log(form())
	}
}
