import { ComponentFixture, TestBed } from '@angular/core/testing'
import { CreateContentType } from './create.component'

describe('ContentTypesFeatCreate', () => {
	let component: CreateContentType
	let fixture: ComponentFixture<CreateContentType>

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [CreateContentType],
		}).compileComponents()

		fixture = TestBed.createComponent(CreateContentType)
		component = fixture.componentInstance
		fixture.detectChanges()
	})

	it('should create', () => {
		expect(component).toBeTruthy()
	})
})
