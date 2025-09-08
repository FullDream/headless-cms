import { Route } from '@angular/router'
import { ContentSchemaProvider } from '@headless-cms/shared/data-access'
import { ContentTypeSchema } from '@headless-cms/content-types/data-access'

export const appRoutes: Route[] = [
	{
		path: 'content-types',
		loadChildren: () => import('@headless-cms/content-types/feat-editor').then(m => m.contentTypeEditorRoutes),
	},
	{
		path: 'content-entries/:typeName',
		providers: [{ provide: ContentSchemaProvider, useClass: ContentTypeSchema }],
		loadComponent: () => import('@headless-cms/content-entries/feat-list').then(m => m.ContentEntriesFeatList),
	},
]
