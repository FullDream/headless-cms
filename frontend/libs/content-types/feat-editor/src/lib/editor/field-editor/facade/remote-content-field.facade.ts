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

	constructor() {
		super()
		this.writableState.connect(
			'contentTypeId',
			inject(ActivatedRoute).paramMap.pipe(map(map => map.get('contentTypeId') || undefined)),
		)
		const contentTypeId = this.writableState.signal('contentTypeId')

		const contentTypeQuery = injectQuery(() => this.#queryOptions.getById(contentTypeId()))

		this.writableState.connect(
			'fields',
			computed(() => contentTypeQuery.data()?.fields || []),
		)

		inject(DestroyRef).onDestroy(() => {
			console.log('destroyed')
		})
	}

	override create(field: CreateContentFieldDto): void {
		const contentTypeId = this.writableState.get('contentTypeId')

		if (!contentTypeId) return

		this.#createMutation.mutate({ contentTypeId, dto: field })
	}

	override update(id: string, field: UpdateContentFieldDto): void {
		const contentTypeId = this.writableState.get('contentTypeId')

		if (!contentTypeId) return

		this.#updateMutation.mutate({ contentTypeId, fieldId: id, dto: field })
	}

	override delete(id: string): void {}
}
