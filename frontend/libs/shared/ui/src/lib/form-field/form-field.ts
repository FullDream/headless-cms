import {
	ChangeDetectionStrategy,
	Component,
	computed,
	contentChild,
	effect,
	ElementRef,
	inject,
	input,
	Signal,
} from '@angular/core'
import { CommonModule } from '@angular/common'
import { FloatLabel } from 'primeng/floatlabel'
import { NgControl } from '@angular/forms'
import { randomString } from 'remeda'
import { getErrorsMessagesFromControl } from '@headless-cms/shared/util-forms'
import { toObservable, toSignal } from '@angular/core/rxjs-interop'
import { of, switchMap } from 'rxjs'
import { ErrorMessage } from '../error-message/error-message'
import { ID_HASH } from '../id-scope/id-hash.token'

@Component({
	selector: 'ui-form-field',
	imports: [CommonModule, FloatLabel, ErrorMessage],
	templateUrl: './form-field.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FormField {
	readonly label = input<string>('')

	readonly id = computed(() => this.ngControl()?.path?.join('-') + this.#hash)

	protected readonly controlEl = contentChild(NgControl, { read: ElementRef })
	protected readonly messages: Signal<string[]>
	protected readonly ngControl = contentChild(NgControl)
	protected readonly tooltip = input<string>()

	readonly #hash = inject(ID_HASH, { optional: true }) ?? randomString(4)

	constructor() {
		this.messages = toSignal(
			toObservable(this.ngControl).pipe(
				switchMap(ngControl => (ngControl?.control ? getErrorsMessagesFromControl(ngControl.control) : of([]))),
			),
			{ initialValue: [] },
		)

		effect(() => {
			const el = this.controlEl()?.nativeElement

			if (el instanceof HTMLInputElement) el.id = this.id()
		})
	}
}
