import { Routes } from '@angular/router'

export const createContentTypeRoutes: Routes = [
	{
		path: 'new',
		title: 'New content type',
		loadComponent: () => import('./create-content-type'),
		children: [
			{
				path: 'new',
				title: 'Add field',
				outlet: 'field',
				loadComponent: () => import('../field-editor/field-editor'),
			},
		],
	},
]
