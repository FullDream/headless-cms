import { TextControlComponent } from './text/text'
import { NumberControl } from './number/number'
import { TextareaControlComponent } from './textarea/textarea'
import { CheckboxControlComponent } from './checkbox/checkbox'

export type DynamicControlType = 'text' | 'number' | 'textarea' | 'checkbox'

export const CONTROL_MAP = {
	text: TextControlComponent,
	number: NumberControl,
	textarea: TextareaControlComponent,
	checkbox: CheckboxControlComponent,
}
