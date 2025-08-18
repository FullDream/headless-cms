import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core'
import { provideRouter, withComponentInputBinding } from '@angular/router'
import { appRoutes } from './app.routes'
import { provideTanStackQuery, QueryClient } from '@tanstack/angular-query-experimental'
import { provideHttpClient, withFetch } from '@angular/common/http'
import { providePrimeNG } from 'primeng/config'
import Aura from '@primeuix/themes/aura'
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async'
import { definePreset } from '@primeuix/themes'

const preset = definePreset(Aura, {
	semantic: {
		primary: {
			50: '{indigo.50}',
			100: '{indigo.100}',
			200: '{indigo.200}',
			300: '{indigo.300}',
			400: '{indigo.400}',
			500: '{indigo.500}',
			600: '{indigo.600}',
			700: '{indigo.700}',
			800: '{indigo.800}',
			900: '{indigo.900}',
			950: '{indigo.950}',
		},
	},
})

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideZonelessChangeDetection(),
		provideRouter(appRoutes, withComponentInputBinding()),
		provideHttpClient(withFetch()),
		provideTanStackQuery(new QueryClient()),
		provideAnimationsAsync(),
		providePrimeNG({
			ripple: true,
			theme: {
				preset: preset,
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
