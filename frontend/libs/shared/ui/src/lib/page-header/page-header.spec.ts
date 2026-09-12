import { Component, signal } from '@angular/core'
import { ComponentPortal } from '@angular/cdk/portal'
import { TestBed } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { provideRouter, Router, RouterLink, RouterOutlet } from '@angular/router'
import { PageHeaderComponent } from './page-header.component'
import { PageHeaderOutlet } from './page-header-outlet'
import { PageHeaderPortal } from './page-header-portal'
import { PageHeaderService } from './page-header.service'

@Component({
	imports: [PageHeaderPortal, RouterLink],
	template: `
		<ng-template uiPageHeaderPortal [breadcrumbs]="[{ label: 'Roles' }]">
			<a routerLink="new">Create role</a>
		</ng-template>
	`,
})
class ListPage {}

@Component({
	imports: [PageHeaderPortal],
	template: `
		<ng-template uiPageHeaderPortal [breadcrumbs]="[{ label: 'Roles', routerLink: '/roles' }, { label: name() }]">
			<button (click)="saves.set(saves() + 1)">Save {{ name() }}</button>
		</ng-template>
	`,
})
class EditorPage {
	readonly name = signal('Loading')
	readonly saves = signal(0)
}

@Component({ template: '' })
class EmptyPage {}

@Component({
	imports: [PageHeaderOutlet, PageHeaderComponent, RouterOutlet],
	template: `
		<div uiPageHeaderOutlet #header="uiPageHeaderOutlet">
			<ui-page-header [breadcrumbs]="header.breadcrumbs()" [portal]="header.portal()" />
			<router-outlet />
		</div>
	`,
})
class TestLayout {}

describe('Page header', () => {
	afterEach(() => TestBed.resetTestingModule())

	it('updates breadcrumbs and actions in the layout and clears them across navigation', async () => {
		TestBed.configureTestingModule({
			imports: [TestLayout],
			providers: [
				provideRouter([
					{ path: 'roles', component: ListPage },
					{ path: 'roles/new', component: EditorPage },
					{ path: 'empty', component: EmptyPage },
				]),
			],
		})
		const fixture = TestBed.createComponent(TestLayout)
		const router = TestBed.inject(Router)
		fixture.detectChanges()
		const navigate = async (url: string): Promise<void> => {
			await router.navigateByUrl(url)
			await fixture.whenStable()
			fixture.detectChanges()
		}
		const header = (): HTMLElement => fixture.nativeElement.querySelector('header')

		await navigate('/roles')
		expect(header().textContent).toContain('Roles')
		expect(header().querySelector('a')?.getAttribute('href')).toBe('/roles/new')

		await navigate('/roles/new')
		expect(header().textContent).not.toContain('Create role')
		const editor = fixture.debugElement.query(By.directive(EditorPage)).componentInstance as EditorPage
		editor.name.set('Administrator')
		await fixture.whenStable()
		fixture.detectChanges()
		expect(header().textContent).toContain('Administrator')
		expect(header().textContent).not.toContain('Loading')
		expect(header().querySelector('a')?.getAttribute('href')).toBe('/roles')
		header().querySelector('button')?.click()
		expect(editor.saves()).toBe(1)

		await navigate('/roles')
		expect(header().textContent).toContain('Create role')
		expect(header().textContent).not.toContain('Administrator')
		expect(header().querySelector('button')).toBeNull()

		await navigate('/empty')
		expect(header().textContent?.trim()).toBe('')
		expect(header().querySelector('a, button')).toBeNull()
	})

	it('ignores updates and destruction of an old portal after a new portal attaches', () => {
		const service = new PageHeaderService()
		const oldPortal = new ComponentPortal(EmptyPage)
		const newPortal = new ComponentPortal(EmptyPage)
		service.attach(oldPortal, [{ label: 'Old' }])
		service.attach(newPortal, [{ label: 'New' }])
		service.updateBreadcrumbs(oldPortal, [{ label: 'Stale' }])
		service.detach(oldPortal)
		expect(service.portal()).toBe(newPortal)
		expect(service.breadcrumbs()).toEqual([{ label: 'New' }])
		service.detach(newPortal)
		expect(service.portal()).toBeNull()
		expect(service.breadcrumbs()).toEqual([])
	})

	it('provides separate state for each outlet', () => {
		TestBed.configureTestingModule({ imports: [TestLayout], providers: [provideRouter([])] })
		const first = TestBed.createComponent(TestLayout)
		const second = TestBed.createComponent(TestLayout)
		first.detectChanges()
		second.detectChanges()
		const serviceFor = (fixture: typeof first): PageHeaderService =>
			fixture.debugElement.query(By.directive(PageHeaderOutlet)).injector.get(PageHeaderService)
		const firstService = serviceFor(first)
		const secondService = serviceFor(second)
		firstService.attach(new ComponentPortal(EmptyPage), [{ label: 'First' }])
		expect(firstService).not.toBe(secondService)
		expect(secondService.portal()).toBeNull()
		expect(secondService.breadcrumbs()).toEqual([])
	})
})
