import { ChangeDetectionStrategy, Component, computed, inject, input, linkedSignal } from '@angular/core'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { DynamicForm, DynamicFormConfig } from '@headless-cms/shared/ui-dynamic-form'
import { form } from '@angular/forms/signals'
import { ButtonDirective } from 'primeng/button'
import { ContentEntriesQueryOptions } from '@headless-cms/content-entries/data-access'
import { mapToObj } from 'remeda'
import { mapFieldToControlConfig } from './map-field-to-control-config'
import { ContentEntryFormModel } from './content-entry.models'
import { RouterLink } from '@angular/router'

@Component({
	selector: 'ce-editor',
	imports: [DynamicForm, ButtonDirective, RouterLink],
	templateUrl: './content-entry-editor.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContentEntryEditor {
	readonly typeName = input.required<string>()

	readonly #schema = inject(ContentSchemaProvider)
	readonly contentEntryOptions = inject(ContentEntriesQueryOptions)

	readonly #schemaQuery = injectQuery(() => this.#schema.getSchemaOptions(this.typeName()))
	readonly createMutation = injectMutation(() => this.contentEntryOptions.create())

	protected readonly dynamicFormConfig = computed<DynamicFormConfig<ContentEntryFormModel>>(
		() => this.#schemaQuery.data()?.fields.map(mapFieldToControlConfig) ?? [],
	)
	protected readonly dynamicFormModel = linkedSignal(() =>
		mapToObj(this.dynamicFormConfig(), controlConfig => [controlConfig.name, controlConfig.value]),
	)
	protected readonly form = form(this.dynamicFormModel)

	protected submit(event: Event): void {
		event.preventDefault()

		this.createMutation.mutate({ typeName: this.typeName(), dto: this.form().value() })
	}
}
