import nx from '@nx/eslint-plugin'
import enforceEsPrivate from 'eslint-plugin-enforce-es-private'

export default [
	...nx.configs['flat/base'],
	...nx.configs['flat/typescript'],
	...nx.configs['flat/javascript'],
	...enforceEsPrivate.configs.recommended,
	{
		ignores: ['**/dist'],
	},
	{
		files: ['**/*.ts', '**/*.tsx', '**/*.js', '**/*.jsx'],
		rules: {
			'@nx/enforce-module-boundaries': [
				'error',
				{
					enforceBuildableLibDependency: true,
					allow: ['^.*/eslint(\\.base)?\\.config\\.[cm]?[jt]s$'],
					depConstraints: [
						{
							sourceTag: 'type:feature',
							onlyDependOnLibsWithTags: ['type:ui', 'type:data-access', 'type:util'],
						},
						{ sourceTag: 'type:ui', onlyDependOnLibsWithTags: ['type:ui', 'type:util'] },
						{ sourceTag: 'type:data-access', onlyDependOnLibsWithTags: ['type:data-access', 'type:util'] },
						{ sourceTag: 'type:util', onlyDependOnLibsWithTags: ['type:util'] },

						// bounded contexts
						{ sourceTag: 'scope:shared', onlyDependOnLibsWithTags: ['scope:shared'] },
						{
							sourceTag: 'scope:content-types',
							onlyDependOnLibsWithTags: ['scope:content-types', 'scope:shared'],
						},
						{
							sourceTag: 'scope:content-entries',
							onlyDependOnLibsWithTags: ['scope:content-entries', 'scope:shared'],
						},
					],
				},
			],
		},
	},
	{
		files: ['**/*.ts', '**/*.cts', '**/*.mts'],
		rules: {
			'@typescript-eslint/interface-name-prefix': 'off',
			'@typescript-eslint/explicit-module-boundary-types': 'off',
			'@typescript-eslint/no-explicit-any': 'error',
			'@typescript-eslint/no-empty-function': 'off',
			'@typescript-eslint/explicit-function-return-type': [
				'error',
				{
					allowExpressions: true,
				},
			],
		},
	},
]
