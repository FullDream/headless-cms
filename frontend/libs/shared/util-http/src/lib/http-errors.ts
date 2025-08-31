import type { HttpErrorResponse } from '@angular/common/http'

export type ApiErrorResponse<M extends Record<number, unknown>> = {
	[S in Extract<keyof M, number>]: Omit<HttpErrorResponse, 'status' | 'error'> & { status: S; error: M[S] }
}[Extract<keyof M, number>]
