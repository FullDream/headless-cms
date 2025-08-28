import { Routes } from '@angular/router'

const children: Routes = [
	{
		path: 'new',
		title: 'Add field',
		outlet: 'field',
		loadComponent: () => import('./field-editor/field-editor'),
	},
	{
		path: ':fieldId',
		title: 'Edit field',
		outlet: 'field',
		loadComponent: () => import('./field-editor/field-editor'),
	},
]

export const contentTypeEditorRoutes: Routes = [
	{
		path: 'new',
		title: 'New content type',
		loadComponent: () => import('././content-type-editor'),
		children,
	},
	{
		path: ':contentTypeId',
		title: 'Edit content type',
		loadComponent: () => import('././content-type-editor'),
		children,
	},
]
