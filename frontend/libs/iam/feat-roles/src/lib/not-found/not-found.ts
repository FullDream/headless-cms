import { Component } from '@angular/core'
import { ButtonDirective } from 'primeng/button'
import { RouterLink } from '@angular/router'

@Component({
	selector: 'iam-not-found',
	imports: [ButtonDirective, RouterLink],
	templateUrl: './not-found.html',
})
export class NotFound {}
