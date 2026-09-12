import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core'
import {
	provideRouter,
	withComponentInputBinding,
	withExperimentalAutoCleanupInjectors,
	withRouterConfig,
} from '@angular/router'
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
import { definePreset } from '@primeuix/themes'

const retryConfig = (failureCount: number, error: Error): boolean => {
	if (
		error instanceof HttpErrorResponse &&
		[
			HttpStatusCode.Unauthorized,
			HttpStatusCode.Conflict,
			HttpStatusCode.BadRequest,
			HttpStatusCode.Forbidden,
		].includes(error.status)
	)
		return false

	return failureCount < 2
}

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideRouter(appRoutes, withComponentInputBinding()),
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
				preset: definePreset(Aura, {
					semantic: {
						primary: {
							50: '{surface.50}',
							100: '{surface.100}',
							200: '{surface.200}',
							300: '{surface.300}',
							400: '{surface.400}',
							500: '{surface.500}',
							600: '{surface.600}',
							700: '{surface.700}',
							800: '{surface.800}',
							900: '{surface.900}',
							950: '{surface.950}',
							color: 'light-dark({primary.950}, {primary.50})',
							contrastColor: 'light-dark(#ffffff, {primary.950})',
							hoverColor: 'light-dark({primary.800}, {primary.200})',
							activeColor: 'light-dark({primary.700}, {primary.300})',
						},
						highlight: {
							background: 'light-dark({primary.950}, {primary.50})',
							focusBackground: 'light-dark({primary.700}, {primary.300})',
							color: 'light-dark(#ffffff, {primary.950})',
							focusColor: 'light-dark(#ffffff, {primary.950})',
						},
					},
				}),
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
