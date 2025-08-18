import { Route } from '@angular/router'

export const appRoutes: Route[] = [
	{
		path: 'content-types',
		loadChildren: () => import('@headless-cms/content-types/feat-create').then(m => m.createContentTypeRoutes),
	},
]
