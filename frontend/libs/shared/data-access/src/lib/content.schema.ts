import { ContentTypeDto, GetContentTypesByNameErrors } from './generated/types.gen'
import { ApiErrorResponse } from '@headless-cms/shared/util-http'

export type ContentSchema = ContentTypeDto
export type ContentSchemaError = ApiErrorResponse<GetContentTypesByNameErrors>
