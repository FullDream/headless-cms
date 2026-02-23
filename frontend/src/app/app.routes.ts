import { Route } from '@angular/router'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { ContentTypeSchema } from '@headless-cms/content-types/data-access'
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
					import('@headless-cms/content-types/feat-editor').then(m => m.contentTypeEditorRoutes),
			},
			{
				path: 'content-entries/:typeName',
				providers: [{ provide: ContentSchemaProvider, useClass: ContentTypeSchema }],
				children: [
					{
						path: '',
						loadComponent: () =>
							import('@headless-cms/content-entries/feat-list').then(m => m.ContentEntriesFeatList),
					},
					{
						path: 'new',
						loadComponent: () =>
							import('@headless-cms/content-entries/feat-editor').then(m => m.ContentEntryEditor),
					},
					{
						path: ':contentEntryId',
						loadComponent: () =>
							import('@headless-cms/content-entries/feat-editor').then(m => m.ContentEntryEditor),
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
