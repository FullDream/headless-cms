import { ChangeDetectionStrategy, Component, computed, input } from '@angular/core'
import { CommonModule } from '@angular/common'
import { Badge } from 'primeng/badge'
import { Message } from 'primeng/message'
import { Tooltip } from 'primeng/tooltip'

@Component({
	selector: 'ui-error-message',
	imports: [CommonModule, Badge, Message, Tooltip],
	templateUrl: './error-message.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorMessage {
	readonly messages = input<string[]>([])

	protected readonly firstMessage = computed(() => this.messages()[0])
	protected readonly otherMessages = computed(() => this.messages().slice(1))
}
