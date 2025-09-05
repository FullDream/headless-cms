import { Directive } from '@angular/core'
import { ID_HASH } from './id-hash.token'
import { randomString } from 'remeda'

@Directive({
	selector: '[uiIdScope]',
	providers: [
		{
			provide: ID_HASH,
			useFactory: () => randomString(4),
		},
	],
})
export class IdScope {}
