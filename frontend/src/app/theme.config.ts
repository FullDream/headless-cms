import Aura from '@primeuix/themes/aura'
import { ThemeType } from 'primeng/config'

export const createThemeConfig = (systemTheme: boolean): ThemeType => ({
	preset: Aura,
	options: {
		darkModeSelector: systemTheme ? undefined : '.dark-mode',
		cssLayer: {
			name: 'primeng',
			order: 'theme, base, primeng',
		},
	},
})
