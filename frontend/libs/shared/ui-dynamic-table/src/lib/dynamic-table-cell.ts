import { Directive, inject, input, TemplateRef } from '@angular/core'

@Directive({
	selector: '[uiDynamicTableCell]',
})
export class DynamicTableCell<T = unknown> {
	readonly column = input.required<string>({ alias: 'uiDynamicTableCell' })

	readonly templateRef = inject<TemplateRef<DynamicTableCellContext<T>>>(TemplateRef)
}

export type DynamicTableCellContext<T> = {
	$implicit: T
	value: unknown
}
