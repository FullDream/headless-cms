import { Route } from '@angular/router'

export const appRoutes: Route[] = [
	{
		path: 'content-types/new',
		loadComponent: () => import('@headless-cms/content-types/feat-create').then(m => m.CreateContentType),
	},
]
