import { inject } from '@angular/core'
import { AsyncValidatorFn } from '@angular/forms'
import { combineLatestWith, map, Observable, take } from 'rxjs'
import { ContentFieldFacade } from './facade/content-field.facade'

export function fieldUniqueNameValidator(excludeId$: Observable<string | undefined>): AsyncValidatorFn {
	const fields$ = inject(ContentFieldFacade).state.select('fields')

	return control => {
		return fields$.pipe(
			combineLatestWith(excludeId$),
			take(1),
			map(([fields, excludeId]) => {
				const exist = fields?.filter(f => f.id !== excludeId).some(f => f.name === control.value)

				return exist ? { unique: true } : null
			}),
		)
	}
}
