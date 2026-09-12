import { defineConfig } from 'orval'

export default defineConfig({
	iam: {
		input: {
			target: 'http://localhost:5139/openapi/v1.json',
			filters: {
				mode: 'include',
				tags: ['AccessResources', 'Auth', 'Roles'],
			},
		},
		output: {
			target: 'src/lib/generated',
			schemas: 'src/lib/generated/models',
			baseUrl: '/api',
			mode: 'tags-split',
			client: 'angular-query',
			httpClient: 'angular',
			namingConvention: 'kebab-case',
			formatter: 'prettier',

			override: {
				namingConvention: {
					enum: 'PascalCase',
				},
			},
		},
	},
})
