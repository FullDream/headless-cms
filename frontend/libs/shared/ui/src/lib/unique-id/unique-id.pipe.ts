import { inject, Pipe, PipeTransform } from '@angular/core'
import { UNIQUE_SUFFIX } from './unique-suffix'

@Pipe({ name: 'uniqueId' })
export class UniqueIdPipe implements PipeTransform {
	readonly #randomSuffix = inject(UNIQUE_SUFFIX)

	transform(value: string): string {
		return `${value}-${this.#randomSuffix}`
	}
}
