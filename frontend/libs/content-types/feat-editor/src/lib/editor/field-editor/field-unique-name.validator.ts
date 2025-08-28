import { inject } from '@angular/core'
import { AsyncValidatorFn } from '@angular/forms'
import { map, take } from 'rxjs'
import { ContentFieldFacade } from './facade/content-field.facade'

export function fieldUniqueNameValidator(excludeId?: string): AsyncValidatorFn {
	const fields$ = inject(ContentFieldFacade).state.select('fields')

	return control => {
		return fields$.pipe(
			take(1),
			map(fields => {
				const exist = fields?.filter(f => f.id !== excludeId).some(f => f.name === control.value)

				return exist ? { fieldNameExists: true } : null
			}),
		)
	}
}
