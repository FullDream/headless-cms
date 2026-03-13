import { Component } from '@angular/core'
import { FormField } from '@angular/forms/signals'
import { InputText } from 'primeng/inputtext'
import { BaseDynamicControl } from '../base-dynamic-control'
import { FloatLabel } from 'primeng/floatlabel'

@Component({
	selector: 'ui-text-control',
	templateUrl: './text.html',
	imports: [InputText, FormField, FloatLabel],
	host: { class: 'block w-full' },
})
export class TextControlComponent extends BaseDynamicControl<string> {}
