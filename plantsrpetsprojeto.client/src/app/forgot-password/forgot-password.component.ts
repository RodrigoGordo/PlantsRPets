import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  message: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  sendResetLink(): void {
    if (this.forgotPasswordForm.invalid) return;

    this.isLoading = true;
    const email = this.forgotPasswordForm.value.email;

    this.http.post('api/forgot-password', { email }).subscribe({
      next: () => {
        this.isLoading = false;
        this.message = "A password reset link has been sent to your email.";
        this.errorMessage = '';
      },
      error: err => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || "An error occurred. Please try again.";
        this.message = '';
      }
    });
  }
}
