import { ContentFieldFacade } from './content-field.facade'
import { injectMutation, injectQuery } from '@tanstack/angular-query-experimental'
import { computed, DestroyRef, inject } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { map } from 'rxjs'
import {
	ContentTypeQueryOptions,
	CreateContentFieldDto,
	UpdateContentFieldDto,
} from '@headless-cms/content-types/data-access'

export class RemoteContentFieldFacade extends ContentFieldFacade {
	readonly #queryOptions = inject(ContentTypeQueryOptions)
	readonly #createMutation = injectMutation(() => this.#queryOptions.createFieldForContentType)
	readonly #updateMutation = injectMutation(() => this.#queryOptions.updateFieldForContentType)
	readonly #deleteMutation = injectMutation(() => this.#queryOptions.deleteField)
	readonly #contentTypeId = this.writableState.signal('contentTypeId')

	constructor() {
		super()
		this.writableState.connect(
			'contentTypeId',
			inject(ActivatedRoute).paramMap.pipe(map(map => map.get('contentTypeId') || undefined)),
		)

		const contentTypeQuery = injectQuery(() => this.#queryOptions.getById(this.#contentTypeId()))

		this.writableState.connect(
			'fields',
			computed(() => contentTypeQuery.data()?.fields || []),
		)

		inject(DestroyRef).onDestroy(() => {
			console.log('destroyed')
		})
	}

	create(field: CreateContentFieldDto): void {
		const contentTypeId = this.writableState.get('contentTypeId')

		if (!contentTypeId) return

		this.#createMutation.mutate({ contentTypeId, dto: field })
	}

	update(id: string, field: UpdateContentFieldDto): void {
		const contentTypeId = this.#contentTypeId()

		if (!contentTypeId) return

		this.#updateMutation.mutate({ contentTypeId, fieldId: id, dto: field })
	}

	delete(id: string): void {
		const contentTypeId = this.#contentTypeId()

		if (contentTypeId) this.#deleteMutation.mutate({ fieldId: id, contentTypeId })
	}
}
