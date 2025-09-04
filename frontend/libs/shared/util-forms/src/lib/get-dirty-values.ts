import { AbstractControl, FormGroup } from '@angular/forms'
import * as R from 'remeda'

type DirtyResult<C> =
	C extends FormGroup<infer G extends Record<string, AbstractControl>>
		? DirtyValues<G>
		: C extends AbstractControl<infer V>
			? V
			: unknown

export type DirtyValues<TControls extends Record<string, AbstractControl>> = {
	[K in keyof TControls]?: DirtyResult<TControls[K]>
}

export function getDirtyValuesForPatch<TControls extends Record<string, AbstractControl>>(
	group: FormGroup<TControls>,
): DirtyValues<TControls> {
	return R.pipe(
		R.pickBy(group.controls, control => control.dirty),
		R.mapValues(control => (control instanceof FormGroup ? getDirtyValuesForPatch(control) : control.value)),
	) as DirtyValues<TControls>
}
