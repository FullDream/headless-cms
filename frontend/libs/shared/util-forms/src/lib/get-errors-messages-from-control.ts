import { AbstractControl } from '@angular/forms'
import { distinctUntilChanged, map, Observable } from 'rxjs'
import * as R from 'remeda'

type errorPayloads = {
	required: true
	requiredTrue: true
	email: true
	minlength: { requiredLength: number; actualLength: number }
	maxlength: { requiredLength: number; actualLength: number }
	min: { min: number; actual: number }
	max: { max: number; actual: number }
	pattern: { requiredPattern: string; actualValue: string }
	server: string[]
	unique: boolean
}

type MsgFn<T> = T extends true ? () => string : (payload: T) => string | string[]

type ErrorMessages<E extends object> = {
	[K in keyof E]-?: MsgFn<NonNullable<E[K]>>
}

const messages: ErrorMessages<errorPayloads> = {
	required: () => 'This field is required.',
	requiredTrue: () => 'You must check this box.',
	email: () => 'Enter a valid email address.',
	minlength: ({ requiredLength, actualLength }) =>
		`Enter at least ${requiredLength} characters (currently ${actualLength}).`,
	maxlength: ({ requiredLength, actualLength }) =>
		`Enter no more than ${requiredLength} characters (currently ${actualLength}).`,
	min: ({ min, actual }) => `Value must be greater than or equal to ${min} (current: ${actual}).`,
	max: ({ max, actual }) => `Value must be less than or equal to ${max} (current: ${actual}).`,
	pattern: () => 'Invalid format.',
	server: messages => messages,
	unique: () => 'This field must be unique.',
}

export function getErrorsMessagesFromControl<TControl extends AbstractControl>(
	control: TControl,
): Observable<string[]> {
	return control.statusChanges.pipe(
		map(() => control.errors),
		distinctUntilChanged(),
		map(errors =>
			!R.isPlainObject(errors)
				? []
				: R.pipe(
						errors,
						R.mapValues((value, key) => messages[key as keyof errorPayloads]?.(value) ?? value),
						R.values(),
						R.flat(),
					),
		),
	)
}
