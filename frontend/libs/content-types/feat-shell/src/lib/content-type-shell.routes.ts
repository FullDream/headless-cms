import { Routes } from '@angular/router'
import { ContentTypeLayout } from './content-type-layout/content-type-layout'
import { ContentTypeIndex } from './content-type-empty/content-type-index'
import { contentTypeEditorRoutes } from '@headless-cms/content-types/feat-editor'

export const contentTypeShellRoutes = [
	{
		path: '',
		component: ContentTypeLayout,
		children: [
			{
				path: '',
				component: ContentTypeIndex,
			},
			...contentTypeEditorRoutes,
		],
	},
] satisfies Routes
