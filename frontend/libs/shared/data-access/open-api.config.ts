import { defineConfig } from '@hey-api/openapi-ts'

export default defineConfig({
	input: 'http://localhost:5139/openapi/v1.json',
	output: 'libs/shared/data-access/src/lib/generated',
	plugins: [
		{
			name: '@hey-api/typescript',
			enums: {
				mode: 'javascript',
				case: 'PascalCase',
			},
			exportFromIndex: false,
		},
	],
})
