import { FormGroup } from '@angular/forms'

export function setFormGroupServerErrors<F extends FormGroup>(form: F, errors: Record<string, string[]>): void {
	for (const [key, messages] of Object.entries(errors)) {
		const control = form.get(key)

		if (!control) continue

		control.setErrors({ server: messages })
		control.updateValueAndValidity({ onlySelf: true, emitEvent: false })
	}
}
