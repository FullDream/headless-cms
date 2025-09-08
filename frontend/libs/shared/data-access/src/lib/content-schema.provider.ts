import { ContentSchema, ContentSchemaError } from './content.schema'
import { CreateQueryOptions } from '@tanstack/angular-query-experimental'

export abstract class ContentSchemaProvider {
	abstract getSchemaOptions(name: string): CreateQueryOptions<ContentSchema, ContentSchemaError>
}
