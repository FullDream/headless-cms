import {
	ApplicationConfig,
	provideBrowserGlobalErrorListeners,
	provideZonelessChangeDetection,
} from '@angular/core'
import { provideRouter } from '@angular/router'
import { appRoutes } from './app.routes'
import { provideTanStackQuery, QueryClient } from '@tanstack/angular-query-experimental'
import { provideHttpClient, withFetch } from '@angular/common/http'

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideHttpClient(withFetch()),
		provideTanStackQuery(new QueryClient()),
		provideRouter(appRoutes),
	],
}
