import { ContentField, FieldType } from '@headless-cms/shared/data-access'
import { DynamicFormConfig } from '@headless-cms/shared/ui-dynamic-form'
import { ContentEntryFormModel } from './content-entry.models'

export function mapFieldToControlConfig(field: ContentField): DynamicFormConfig<ContentEntryFormModel>[number] {
	switch (field.type) {
		case FieldType.Integer:
		case FieldType.Decimal:
			return {
				type: 'number',
				value: 0,
				name: field.name,
				label: field.label ?? field.name,
			}
		case FieldType.LongText:
			return {
				type: 'textarea',
				value: '',
				name: field.name,
				label: field.label ?? field.name,
			}
		case FieldType.Boolean:
			return {
				type: 'checkbox',
				value: false,
				name: field.name,
				label: field.label ?? field.name,
			}
		case FieldType.ShortText:
		default:
			return {
				type: 'text',
				value: '',
				name: field.name,
				label: field.label ?? field.name,
			}
	}
}
