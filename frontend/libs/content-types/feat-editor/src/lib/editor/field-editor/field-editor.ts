import { Component, effect, inject, input } from '@angular/core'
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { UniqueIdPipe, UniqueIdScopeDirective } from '@headless-cms/shared/ui'
import { Select } from 'primeng/select'
import { Checkbox } from 'primeng/checkbox'
import { Button } from 'primeng/button'
import { AutoFocus } from 'primeng/autofocus'
import { fieldUniqueNameValidator } from './field-unique-name.validator'
import { ActivatedRoute, Router } from '@angular/router'
import { ContentFieldFacade } from './facade/content-field.facade'
import { FieldType } from '@headless-cms/content-types/data-access'
import { getDirtyValuesForPatch } from '@headless-cms/shared/util-forms'

@Component({
	selector: 'ct-edit-field',
	templateUrl: './field-editor.html',
	imports: [ReactiveFormsModule, FloatLabel, InputText, UniqueIdPipe, Select, Checkbox, AutoFocus, Button],
	hostDirectives: [UniqueIdScopeDirective],
	host: {
		class: 'p-2',
	},
})
export default class FieldEditor {
	protected readonly fieldId = input<string>()
	protected readonly fieldTypes: FieldType[] = Object.values(FieldType)
	readonly #fb = inject(FormBuilder)
	// protected readonly kebabChar = /[a-z0-9-]/

	readonly #facade = inject(ContentFieldFacade)
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
		const currentField = this.#facade.state.computed(state => state.fields().find(f => f.id === this.fieldId()))

		effect(() => currentField() && this.form.reset(currentField(), { emitEvent: false }))
	}

	protected submit(): void {
		if (this.form.invalid) return

		const id = this.fieldId()

		if (id) this.#facade.update(id, getDirtyValuesForPatch(this.form))
		else this.#facade.create(this.form.getRawValue())

		void this.#router.navigate(['../'], { relativeTo: this.#route })
	}

	protected delete(id: string): void {
		this.#facade.delete(id)

		void this.#router.navigate(['../'], { relativeTo: this.#route })
	}
}
