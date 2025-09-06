import {FieldType} from '@headless-cms/content-types/data-access'

export const fieldTypeNames: Record<FieldType, string> = {
	[FieldType.Boolean]: 'Boolean',
	[FieldType.Decimal]: 'Decimal',
	[FieldType.Integer]: 'Integer',
	[FieldType.LongText]: 'Long text',
	[FieldType.ShortText]: 'Short text',
}
