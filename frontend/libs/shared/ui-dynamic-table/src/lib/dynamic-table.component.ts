import {ChangeDetectionStrategy, Component} from '@angular/core'
import {TableModule} from 'primeng/table'

@Component({
	selector: 'ui-dynamic-table',
	imports: [TableModule],
	templateUrl: './dynamic-table.component.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicTable {}
