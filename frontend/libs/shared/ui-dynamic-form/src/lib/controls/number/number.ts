import { Component } from '@angular/core'
import { BaseDynamicControl } from '../base-dynamic-control'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { FormField } from '@angular/forms/signals'

@Component({
	selector: 'ui-number-control',
	imports: [FloatLabel, InputText, FormField],
	templateUrl: './number.html',
	host: { class: 'block w-full' },
})
export class NumberControl extends BaseDynamicControl<number> {}
