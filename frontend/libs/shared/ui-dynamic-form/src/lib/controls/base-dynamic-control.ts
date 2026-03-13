import { Directive, input } from '@angular/core'
import { Field } from '@angular/forms/signals'

@Directive()
export abstract class BaseDynamicControl<T = string> {
	public readonly label = input('')
	public readonly field = input.required<Field<T>>()
}
