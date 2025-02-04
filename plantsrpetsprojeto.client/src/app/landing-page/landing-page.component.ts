import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})

export class LandingPageComponent {

  constructor(private router: Router) { }

  goToForgotPassword(): void {
    this.router.navigate(['/forgot-password']);
  }
}
