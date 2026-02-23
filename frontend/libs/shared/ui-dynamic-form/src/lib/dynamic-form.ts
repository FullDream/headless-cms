import { ChangeDetectionStrategy, Component, input, linkedSignal } from '@angular/core'
import { mapToObj } from 'remeda'
import { form, FormField } from '@angular/forms/signals'
import { InputText } from 'primeng/inputtext'
import { FloatLabel } from 'primeng/floatlabel'

type DynamicControlConfig<T = string> = {
	type: 'number' | 'string' | 'textarea'
	name: string
	value: T
	label: string
}

export type DynamicFormConfig = DynamicControlConfig[]

@Component({
	selector: 'ui-dynamic-form',
	templateUrl: './dynamic-form.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
	exportAs: 'dynamicForm',
	imports: [InputText, FormField, FloatLabel],
})
export class DynamicForm {
	public readonly config = input.required<DynamicFormConfig>()

	protected readonly model = linkedSignal(() =>
		mapToObj(this.config(), controlConfig => [controlConfig.name, controlConfig.value]),
	)

	public readonly form = form(this.model)
}
