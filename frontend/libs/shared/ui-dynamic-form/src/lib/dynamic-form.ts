import { ChangeDetectionStrategy, Component, input } from '@angular/core'
import { FieldTree } from '@angular/forms/signals'
import { NgComponentOutlet } from '@angular/common'
import { CONTROL_MAP } from './controls/control-map'

type DynamicControlConfig<TForm, TValue> = {
	name: keyof TForm
	value: TValue
	label: string
}

type TextControlConfig<TForm> = DynamicControlConfig<TForm, string> & {
	type: 'text'
}

type NumberControlConfig<TForm> = DynamicControlConfig<TForm, number> & {
	type: 'number'
}

type TextareaControlConfig<TForm> = DynamicControlConfig<TForm, string> & {
	type: 'textarea'
}

type CheckboxControlConfig<TForm> = DynamicControlConfig<TForm, boolean> & {
	type: 'checkbox'
}

export type DynamicFormConfig<TForm> = (
	| TextControlConfig<TForm>
	| NumberControlConfig<TForm>
	| TextareaControlConfig<TForm>
	| CheckboxControlConfig<TForm>
)[]

@Component({
	selector: 'ui-dynamic-form',
	templateUrl: './dynamic-form.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
	exportAs: 'dynamicForm',
	imports: [NgComponentOutlet],
})
export class DynamicForm<T extends Record<string, unknown>> {
	public readonly config = input.required<DynamicFormConfig<T>>()
	public readonly form = input.required<FieldTree<T>>()

	protected readonly controls = CONTROL_MAP
}
