import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core'
import { provideRouter } from '@angular/router'
import { appRoutes } from './app.routes'
import { provideTanStackQuery, QueryClient } from '@tanstack/angular-query-experimental'
import { provideHttpClient, withFetch } from '@angular/common/http'
import { providePrimeNG } from 'primeng/config'
import Aura from '@primeuix/themes/aura'
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideRouter(appRoutes),
		provideHttpClient(withFetch()),
		provideTanStackQuery(new QueryClient()),
		provideAnimationsAsync(),
		providePrimeNG({
			ripple: true,
			theme: {
				preset: Aura,
				options: {
					darkModeSelector: '.dark-mode',
					cssLayer: {
						name: 'primeng',
						order: 'theme, base, primeng',
					},
				},
			},
		}),
	],
}
