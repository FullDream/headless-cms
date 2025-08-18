import { Component, inject } from '@angular/core'
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { FieldType } from '@headless-cms/shared/util-api-types'
import { UniqueIdPipe, UniqueIdScopeDirective } from '@headless-cms/shared/ui'
import { Select } from 'primeng/select'
import { Checkbox } from 'primeng/checkbox'
import { KeyFilter } from 'primeng/keyfilter'
import { ButtonDirective } from 'primeng/button'
import { AutoFocus } from 'primeng/autofocus'

@Component({
	selector: 'ct-edit-field',
	templateUrl: './field-editor.html',
	imports: [
		ReactiveFormsModule,
		FloatLabel,
		InputText,
		UniqueIdPipe,
		Select,
		Checkbox,
		KeyFilter,
		AutoFocus,
		ButtonDirective,
	],
	hostDirectives: [UniqueIdScopeDirective],
	host: {
		class: 'p-2',
	},
})
export default class FieldEditor {
	protected readonly kebabChar = /[a-z0-9-]/

	protected readonly form = inject(FormBuilder).group({
		name: ['', [Validators.required]],
		type: new FormControl<FieldType>('shortText', [Validators.required]),
		isRequired: [false],
	})

	protected submit(): void {
		console.log(this.form.value)
	}
	protected readonly fieldTypes: FieldType[] = ['boolean', 'shortText', 'longText', 'decimal', 'integer']
}
