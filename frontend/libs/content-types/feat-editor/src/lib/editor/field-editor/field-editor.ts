import { Component, effect, inject, input } from '@angular/core'
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { UniqueIdPipe, UniqueIdScopeDirective } from '@headless-cms/shared/ui'
import { Select } from 'primeng/select'
import { Checkbox } from 'primeng/checkbox'
import { ButtonDirective } from 'primeng/button'
import { AutoFocus } from 'primeng/autofocus'
import { fieldUniqueNameValidator } from './field-unique-name.validator'
import { ActivatedRoute, Router } from '@angular/router'
import { ContentFieldFacade } from './facade/content-field.facade'
import { FieldType } from '@headless-cms/content-types/data-access'

@Component({
	selector: 'ct-edit-field',
	templateUrl: './field-editor.html',
	imports: [ReactiveFormsModule, FloatLabel, InputText, UniqueIdPipe, Select, Checkbox, AutoFocus, ButtonDirective],
	hostDirectives: [UniqueIdScopeDirective],
	host: {
		class: 'p-2',
	},
})
export default class FieldEditor {
	protected readonly contentTypeId = input<string>()
	protected readonly fieldId = input<string>()

	readonly #bridge = inject(ContentFieldFacade)
	readonly #fb = inject(FormBuilder)
	// protected readonly kebabChar = /[a-z0-9-]/

	readonly #route = inject(ActivatedRoute)
	readonly #router = inject(Router)

	protected readonly form = this.#fb.nonNullable.group({
		name: this.#fb.nonNullable.control('', {
			validators: [Validators.required],
			asyncValidators: [fieldUniqueNameValidator(this.fieldId())],
		}),
		label: ['', Validators.required],
		type: this.#fb.nonNullable.control<FieldType>('shortText', Validators.required),
		isRequired: [false],
	})

	constructor() {
		const currentField = this.#bridge.state.computed(state => state.fields().find(f => f.id === this.fieldId()))

		effect(() => currentField() && this.form.reset(currentField(), { emitEvent: false }))
	}

	protected submit(): void {
		if (this.form.invalid) return

		const id = this.fieldId()

		if (id) this.#bridge.update(id, this.form.getRawValue())
		else this.#bridge.create(this.form.getRawValue())

		this.form.reset()

		void this.#router.navigate(['../'], { relativeTo: this.#route })
	}
	protected readonly fieldTypes: FieldType[] = Object.values(FieldType)
}
