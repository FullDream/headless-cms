import { Component } from '@angular/core'
import { FormField } from '@angular/forms/signals'
import { Checkbox } from 'primeng/checkbox'
import { BaseDynamicControl } from '../base-dynamic-control'

@Component({
	selector: 'ui-checkbox-control',
	templateUrl: './checkbox.html',
	imports: [FormField, Checkbox],
	host: { class: 'block w-full' },
})
export class CheckboxControlComponent extends BaseDynamicControl<boolean> {}
