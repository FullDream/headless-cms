import { Component, inject } from '@angular/core'
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms'
import { FloatLabel } from 'primeng/floatlabel'
import { InputText } from 'primeng/inputtext'
import { ContentTypeKind } from '@headless-cms/shared/util-api-types'
import { RadioButton } from 'primeng/radiobutton'
import { Divider } from 'primeng/divider'
import { Drawer } from 'primeng/drawer'
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router'
import { toSignal } from '@angular/core/rxjs-interop'
import { UniqueIdPipe, UniqueIdScopeDirective } from '@headless-cms/shared/ui'

@Component({
	selector: 'ct-create-content-type',
	templateUrl: './create-content-type.html',
	imports: [
		ReactiveFormsModule,
		FloatLabel,
		InputText,
		RadioButton,
		Divider,
		Drawer,
		RouterOutlet,
		RouterLink,
		UniqueIdPipe,
	],
	hostDirectives: [UniqueIdScopeDirective],
})
export default class CreateContentType {
	readonly #route = inject(ActivatedRoute)
	readonly #router = inject(Router)

	protected readonly title = toSignal(this.#route.title)
	protected readonly fields = new FormArray([])

	readonly form = inject(FormBuilder).group({
		name: ['', [Validators.required]],
		kind: new FormControl<ContentTypeKind>('collection', [Validators.required]),
		fields: this.fields,
	})

	protected hideOutlet(): void {
		void this.#router.navigate([{ outlets: { field: null } }], { relativeTo: this.#route })
	}
}
