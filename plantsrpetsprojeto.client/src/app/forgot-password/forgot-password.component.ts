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

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  sendResetLink(): void {
    if (this.forgotPasswordForm.invalid) return;

    const email = this.forgotPasswordForm.value.email;

    this.http.post('api/forgot-password', { email }).subscribe({
      next: () => {
        this.message = "Um link de redefinição de senha foi enviado para o seu email.";
        this.errorMessage = '';
      },
      error: err => {
        this.errorMessage = err.error?.message || "Ocorreu um erro. Tente novamente.";
        this.message = '';
      }
    });
  }
}
