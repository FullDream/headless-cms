import { ContentFieldFacade } from './content-field.facade'
import { ContentFieldDto, CreateContentFieldDto, UpdateContentFieldDto } from '@headless-cms/content-types/data-access'

export class LocalContentFieldFacade extends ContentFieldFacade {
	override create(field: CreateContentFieldDto): void {
		this.writableState.set('fields', state => [...state.fields, { ...field, id: `tmp_${crypto.randomUUID()}` }])
	}

	override update(id: string, field: UpdateContentFieldDto): void {
		this.writableState.set('fields', state =>
			state.fields.map(f => (f.id == id ? ({ ...f, ...field } as ContentFieldDto) : f)),
		)
	}

	override delete(id: string): void {
		this.writableState.set('fields', state => state.fields.filter(f => f.id !== id))
	}
}
