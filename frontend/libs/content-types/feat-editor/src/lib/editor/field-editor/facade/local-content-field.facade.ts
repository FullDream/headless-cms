import { ContentFieldFacade } from './content-field.facade'
import { ContentFieldDto, CreateContentFieldDto, UpdateContentFieldDto } from '@headless-cms/content-types/data-access'

export class LocalContentFieldFacade extends ContentFieldFacade {
	create(field: CreateContentFieldDto): void {
		this.writableState.set('fields', state => [...state.fields, { ...field, id: `tmp_${crypto.randomUUID()}` }])
	}

	update(id: string, field: UpdateContentFieldDto): void {
		this.writableState.set('fields', state =>
			state.fields.map(f => (f.id == id ? ({ ...f, ...field } as ContentFieldDto) : f)),
		)
	}

	delete(id: string): void {
		this.writableState.set('fields', state => state.fields.filter(f => f.id !== id))
	}
}
