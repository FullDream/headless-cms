import { Routes } from '@angular/router'
import { RoleEditor } from './role-editor/role-editor'
import { RoleList } from './role-list/role-list'
import { inject } from '@angular/core'
import { QueryClient } from '@tanstack/angular-query-experimental'
import { RolesQueryOptions } from '@headless-cms/iam/data-access'
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http'
import { NotFound } from './not-found/not-found'

export const rolesRoutes = [
	{
		path: '',
		pathMatch: 'full',
		component: RoleList,
	},
	{
		path: ':roleId',
		component: RoleEditor,
		canMatch: [
			async (route, segments, currentSnapshot) => {
				const roleId = currentSnapshot.paramMap.get('roleId')

				if (!roleId?.trim()) return false

				if (roleId === 'new') return true

				const queryClient = inject(QueryClient)
				const rolesQueries = inject(RolesQueryOptions)

				try {
					await queryClient.ensureQueryData(rolesQueries.getById(roleId))

					return true
				} catch (error) {
					if (error instanceof HttpErrorResponse && error.status === HttpStatusCode.NotFound) {
						return false
					}

					throw error
				}
			},
		],
	},
	{
		path: ':roleId',
		loadComponent: () => import('./not-found/not-found').then(m => m.NotFound),
	},
] satisfies Routes
