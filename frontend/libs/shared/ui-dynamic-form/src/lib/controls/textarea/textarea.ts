import { Component } from '@angular/core'
import { FormField } from '@angular/forms/signals'
import { FloatLabel } from 'primeng/floatlabel'
import { Textarea } from 'primeng/textarea'
import { BaseDynamicControl } from '../base-dynamic-control'

@Component({
	selector: 'ui-textarea-control',
	templateUrl: './textarea.html',
	imports: [FormField, FloatLabel, Textarea],
	host: { class: 'block w-full' },
})
export class TextareaControlComponent extends BaseDynamicControl<string> {}
