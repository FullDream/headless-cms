import { Directive } from '@angular/core'
import { UNIQUE_SUFFIX } from './unique-suffix'

@Directive({
	selector: '[sharedUniqueIdScope]',
	providers: [{ provide: UNIQUE_SUFFIX, useFactory: () => Math.random().toString(36).slice(2, 7) }],
})
export class UniqueIdScopeDirective {}
