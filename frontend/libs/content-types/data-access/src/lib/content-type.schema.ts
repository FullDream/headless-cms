import { inject, Injectable } from '@angular/core'
import { CreateQueryOptions } from '@tanstack/angular-query-experimental'
import { ContentTypeQueryOptions } from './content-type.query-options'
import {
	ContentSchema,
	ContentSchemaError,
	ContentSchemaListError,
	ContentSchemaProvider,
} from '@headless-cms/shared/data-access'

@Injectable()
export class ContentTypeSchema extends ContentSchemaProvider {
	#queryOptions = inject(ContentTypeQueryOptions)

	getSchemaListOptions(): CreateQueryOptions<ContentSchema[], ContentSchemaListError> {
		return this.#queryOptions.list
	}

	getSchemaOptions(name: string): CreateQueryOptions<ContentSchema, ContentSchemaError> {
		return this.#queryOptions.getById(name)
	}
}
