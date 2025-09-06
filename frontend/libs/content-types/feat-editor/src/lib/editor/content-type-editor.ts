import { Component, effect, inject, input } from '@angular/core'
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms'
import { InputText } from 'primeng/inputtext'
import { RadioButton } from 'primeng/radiobutton'
import { Drawer } from 'primeng/drawer'
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router'
import { toSignal } from '@angular/core/rxjs-interop'
import { FormField, IdScope, UniqueIdPipe, UniqueIdScopeDirective } from '@headless-cms/shared/ui'
import { ContentFieldDto, ContentTypeKind, ContentTypeQueryOptions } from '@headless-cms/content-types/data-access'
import { Button } from 'primeng/button'
import { ConfirmationService } from 'primeng/api'
import { ConfirmDialog } from 'primeng/confirmdialog'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { ContentFieldFacade } from './field-editor/facade/content-field.facade'
import { RemoteContentFieldFacade } from './field-editor/facade/remote-content-field.facade'
import { LocalContentFieldFacade } from './field-editor/facade/local-content-field.facade'
import { getDirtyValuesForPatch, setFormGroupServerErrors } from '@headless-cms/shared/util-forms'
import { fieldTypeNames } from './field-type-names'
import { KeyFilter } from 'primeng/keyfilter'

@Component({
	selector: 'ct-create-content-type',
	templateUrl: './content-type-editor.html',
	imports: [
		ReactiveFormsModule,
		InputText,
		RadioButton,
		Drawer,
		RouterOutlet,
		RouterLink,
		UniqueIdPipe,
		ConfirmDialog,
		Button,
		FormField,
		KeyFilter,
	],
	hostDirectives: [IdScope, UniqueIdScopeDirective],
	providers: [
		ConfirmationService,
		{
			provide: ContentFieldFacade,
			useFactory: () => {
				return inject(ActivatedRoute).snapshot.paramMap.get('contentTypeId')
					? new RemoteContentFieldFacade()
					: new LocalContentFieldFacade()
			},
		},
	],
})
export default class ContentTypeEditor {
	protected readonly contentTypeId = input<string>()

	readonly #bridge = inject(ContentFieldFacade)
	readonly #data = inject(ContentTypeQueryOptions)
	readonly #fb = inject(FormBuilder)
	readonly #route = inject(ActivatedRoute)
	readonly #router = inject(Router)

	readonly #confirmationService = inject(ConfirmationService)

	protected readonly title = toSignal(this.#route.title)

	readonly form = this.#fb.nonNullable.group({
		name: ['', [Validators.required]],
		kind: this.#fb.nonNullable.control<ContentTypeKind>('collection', [Validators.required]),
		fields: this.#fb.nonNullable.control<ContentFieldDto[]>([]),
	})

	protected readonly kebabChar = /[a-z0-9-]/
	protected readonly fieldTypeNames = fieldTypeNames
	protected readonly fields = this.form.controls.fields
	protected readonly create = injectMutation(() => this.#data.create)
	protected readonly remove = injectMutation(() => this.#data.delete)
	protected readonly contentType = injectQuery(() => this.#data.getById(this.contentTypeId()))
	protected readonly update = injectMutation(() => this.#data.update)

	constructor() {
		const fields = this.#bridge.state.signal('fields')

		effect(() => this.fields.reset(fields()))
		effect(() => this.form.reset(this.contentType.data()))
		effect(() => {
			const error = this.update.error() ?? this.create.error()

			switch (error?.status) {
				case 400:
				case 409:
				case 422:
					if (error.error.errors) setFormGroupServerErrors(this.form, error.error.errors)
			}
		})
	}

	protected updateContentType(): void {
		if (this.form.invalid) return

		const id = this.contentTypeId()

		if (!id) return

		this.update.mutate({ id, dto: getDirtyValuesForPatch(this.form) })
	}

	protected createContentType(): void {
		if (this.form.invalid) return

		const id = this.contentTypeId()
		const { name, fields, kind } = this.form.getRawValue()

		if (id) return this.update.mutate({ id, dto: { name, kind } })

		this.create
			.mutateAsync({ name, kind, fields })
			.then(({ id }) => this.#router.navigate([id], { relativeTo: this.#route.parent, replaceUrl: true }))
	}

	protected onDelete(): void {
		const id = this.contentTypeId()
		if (!id) return

		this.#confirmationService.confirm({
			key: this.contentTypeId(),
			header: `Delete ${this.contentType.data()?.name ?? 'content type'}?`,
			message: 'Are you sure you want to delete this content type?',
			icon: 'pi pi-info-circle',
			rejectButtonProps: {
				label: 'Cancel',
				severity: 'secondary',
				outlined: true,
			},
			acceptButtonProps: {
				label: 'Delete',
				severity: 'danger',
			},
			accept: () =>
				this.remove
					.mutateAsync(id)
					.then(() => this.#router.navigate(['../'], { relativeTo: this.#route, replaceUrl: true })),
		})
	}

	protected hideOutlet(): void {
		void this.#router.navigate([{ outlets: { field: null } }], { relativeTo: this.#route })
	}
}
