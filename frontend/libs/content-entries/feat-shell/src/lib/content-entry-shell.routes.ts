import { Routes } from '@angular/router'
import { ContentEntryEditor } from '@headless-cms/content-entries/feat-editor'
import { ContentEntriesFeatList } from '@headless-cms/content-entries/feat-list'
import { ContentEntryEmpty } from './content-entry-empty/content-entry-empty'
import { ContentEntryLayout } from './content-entry-layout/content-entry-layout'

export const contentEntryShellRoutes = [
	{
		path: '',
		component: ContentEntryLayout,
		children: [
			{
				path: '',
				component: ContentEntryEmpty,
			},
			{
				path: ':typeName',
				children: [
					{
						path: '',
						component: ContentEntriesFeatList,
					},
					{
						path: 'new',
						component: ContentEntryEditor,
					},
					{
						path: ':contentEntryId',
						component: ContentEntryEditor,
					},
				],
			},
		],
	},
] satisfies Routes
