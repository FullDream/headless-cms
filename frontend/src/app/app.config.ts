import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core'
import { provideRouter, withComponentInputBinding } from '@angular/router'
import { appRoutes } from './app.routes'
import { provideTanStackQuery, QueryClient, withDevtools } from '@tanstack/angular-query-experimental'
import { provideHttpClient, withFetch } from '@angular/common/http'
import { providePrimeNG } from 'primeng/config'
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'
import { createThemeConfig } from './theme.config'
import { HubConnectionFactory } from '@ssv/signalr-client'

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideRouter(appRoutes, withComponentInputBinding()),
		provideHttpClient(withFetch()),
		provideTanStackQuery(new QueryClient(), withDevtools()),
		provideAnimationsAsync(),
		HubConnectionFactory,
		providePrimeNG({
			ripple: true,
			theme: createThemeConfig(true),
		}),
	],
}
