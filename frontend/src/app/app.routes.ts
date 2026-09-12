import { Route } from '@angular/router'
import { ContentTypeSchema } from '@headless-cms/content-types/data-access'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { LayoutComponent } from './layout/layout'

export const appRoutes: Route[] = [
	{
		path: '',
		component: LayoutComponent,
		children: [
			{ path: '', pathMatch: 'full', redirectTo: 'content-types' },

			{
				path: 'content-types',

				loadChildren: () =>
					import('@headless-cms/content-types/feat-shell').then(m => m.contentTypeShellRoutes),
			},
			{
				path: 'content-entries',
				providers: [{ provide: ContentSchemaProvider, useClass: ContentTypeSchema }],
				loadChildren: () =>
					import('@headless-cms/content-entries/feat-shell').then(m => m.contentEntryShellRoutes),
			},
			{
				path: 'settings',
				loadComponent: () => import('./settings/settings').then(m => m.Settings),
				children: [
					{ path: '', pathMatch: 'full', redirectTo: 'roles' },
					{
						path: 'roles',
						loadChildren: () => import('@headless-cms/iam/feat-roles').then(m => m.rolesRoutes),
					},
				],
			},
		],
	},
	{
		path: 'auth/login',
		loadComponent: () => import('@headless-cms/iam/feat-auth').then(m => m.AuthLoginComponent),
	},
]
