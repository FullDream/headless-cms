import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http'
import { inject } from '@angular/core'
import { Router } from '@angular/router'
import { catchError, throwError } from 'rxjs'

export const authInterceptor: HttpInterceptorFn = (req, next) => {
	const router = inject(Router)
	const loginRoute = 'auth/login'

	return next(req).pipe(
		catchError((error: HttpErrorResponse) => {
			if (error.status === HttpStatusCode.Unauthorized && !router.url.includes(loginRoute))
				void router.navigate([loginRoute], { queryParams: { returnUrl: router.url } })

			return throwError(() => error)
		}),
	)
}
