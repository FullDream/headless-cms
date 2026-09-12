/// <reference types='vitest' />
import { defineConfig } from 'vite'
import { resolve } from 'node:path'
import angular from '@analogjs/vite-plugin-angular'
import { nxViteTsPaths } from '@nx/vite/plugins/nx-tsconfig-paths.plugin'
import { nxCopyAssetsPlugin } from '@nx/vite/plugins/nx-copy-assets.plugin'

export default defineConfig(() => ({
	root: __dirname,
	cacheDir: '../../../node_modules/.vite/libs/iam/feat-roles',
	plugins: [
		angular({ tsconfig: resolve(__dirname, 'tsconfig.spec.json') }),
		nxViteTsPaths(),
		nxCopyAssetsPlugin(['*.md']),
	],
	// Uncomment this if you are using workers.
	// worker: {
	//   plugins: () => [ nxViteTsPaths() ],
	// },
	test: {
		name: 'feat-roles',
		watch: false,
		globals: true,
		environment: 'jsdom',
		include: ['{src,tests}/**/*.{test,spec}.{js,mjs,cjs,ts,mts,cts,jsx,tsx}'],
		setupFiles: ['src/test-setup.ts'],
		reporters: ['default'],
		coverage: {
			reportsDirectory: '../../../coverage/libs/iam/feat-roles',
			provider: 'v8' as const,
		},
	},
}))
