import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core'
import { provideRouter, withComponentInputBinding, withExperimentalAutoCleanupInjectors } from '@angular/router'
import { appRoutes } from './app.routes'
import { provideTanStackQuery, QueryClient } from '@tanstack/angular-query-experimental'
import { HttpErrorResponse, HttpStatusCode, provideHttpClient, withInterceptors } from '@angular/common/http'
import { providePrimeNG } from 'primeng/config'
import { HubConnectionFactory } from '@ssv/signalr-client'
import { withDevtools } from '@tanstack/angular-query-experimental/devtools'
import { authInterceptor } from '@headless-cms/iam/data-access'
import { provideSignalFormsConfig } from '@angular/forms/signals'
import { NG_STATUS_CLASSES } from '@angular/forms/signals/compat'
import Aura from '@primeuix/themes/aura'

const retryConfig = (failureCount: number, error: Error): boolean => {
	if (error instanceof HttpErrorResponse && error.status === HttpStatusCode.Unauthorized) return false

	return failureCount < 2
}

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideRouter(appRoutes, withComponentInputBinding(), withExperimentalAutoCleanupInjectors()),
		provideHttpClient(withInterceptors([authInterceptor])),
		provideTanStackQuery(
			new QueryClient({ defaultOptions: { queries: { retry: retryConfig }, mutations: { retry: retryConfig } } }),
			withDevtools(),
		),
		HubConnectionFactory,
		provideSignalFormsConfig({
			classes: NG_STATUS_CLASSES,
		}),
		providePrimeNG({
			ripple: true,
			theme: {
				preset: Aura,
				options: {
					cssLayer: {
						name: 'primeng',
						order: 'theme, base, primeng',
					},
				},
			},
		}),
	],
}
