import { ContentFieldDto, ContentTypeDto, GetContentTypesByNameErrors } from './generated/types.gen'
import { ApiErrorResponse } from '@headless-cms/shared/util-http'

export { FieldType } from './generated/types.gen'
export type ContentSchema = ContentTypeDto
export type ContentField = ContentFieldDto
export type ContentSchemaError = ApiErrorResponse<GetContentTypesByNameErrors>
