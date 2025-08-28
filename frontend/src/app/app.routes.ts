import { Route } from '@angular/router'

export const appRoutes: Route[] = [
	{
		path: 'content-types',
		loadChildren: () => import('@headless-cms/content-types/feat-editor').then(m => m.contentTypeEditorRoutes),
	},
]
