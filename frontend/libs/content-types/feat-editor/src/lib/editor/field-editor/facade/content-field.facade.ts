import { rxState } from '@rx-angular/state'
import { ContentFieldDto, CreateContentFieldDto, UpdateContentFieldDto } from '@headless-cms/content-types/data-access'

export abstract class ContentFieldFacade {
	protected readonly writableState = rxState<{ fields: ContentFieldDto[]; contentTypeId?: string }>(({ set }) => {
		set({ fields: [] })
	})

	readonly state = this.writableState.asReadOnly()

	abstract create(field: CreateContentFieldDto): void
	abstract update(id: string, field: UpdateContentFieldDto): void
	abstract delete(id: string): void
}
