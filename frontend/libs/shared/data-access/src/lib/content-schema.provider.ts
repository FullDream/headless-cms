import { ContentSchema, ContentSchemaError, ContentSchemaListError } from './content.schema'
import { CreateQueryOptions } from '@tanstack/angular-query-experimental'

export abstract class ContentSchemaProvider {
	abstract getSchemaListOptions(): CreateQueryOptions<ContentSchema[], ContentSchemaListError>

	abstract getSchemaOptions(name: string): CreateQueryOptions<ContentSchema, ContentSchemaError>
}
