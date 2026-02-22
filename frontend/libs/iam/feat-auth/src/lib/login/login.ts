import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core'
import { InputText } from 'primeng/inputtext'
import { email, form, FormField, required, submit } from '@angular/forms/signals'
import { injectMutation } from '@tanstack/angular-query-experimental'
import { AuthOptions } from '@headless-cms/iam/data-access'
import { ButtonDirective } from 'primeng/button'
import { injectQueryParams } from 'ngxtension/inject-query-params'
import { Router } from '@angular/router'
import { FloatLabel } from 'primeng/floatlabel'
import { Password } from 'primeng/password'

@Component({
	selector: 'iam-auth-login',
	imports: [InputText, FormField, ButtonDirective, FloatLabel, Password],
	templateUrl: './login.html',
	changeDetection: ChangeDetectionStrategy.OnPush,
	host: { class: 'block h-dvh w-dvw flex items-center justify-center bg-gray-50 px-4' },
})
export class AuthLoginComponent {
	readonly #authOptions = inject(AuthOptions)
	readonly #returnUrl = injectQueryParams('returnUrl')
	readonly router = inject(Router)

	protected readonly loginModel = signal({ email: '', password: '' })
	protected readonly form = form(this.loginModel, schemaPath => {
		required(schemaPath.email, { message: 'Email is required' })
		email(schemaPath.email, { message: 'Enter a valid email address' })
		required(schemaPath.password, { message: 'Password is required' })
	})

	protected readonly loginMutation = injectMutation(() => this.#authOptions.login())

	protected onLogin(event: Event): void {
		event.preventDefault()

		if (this.form().invalid()) return

		void submit(this.form, async () => {
			await this.loginMutation.mutateAsync(this.form().value())
			await this.router.navigateByUrl(this.#returnUrl() ?? '/')
		})
	}
}
