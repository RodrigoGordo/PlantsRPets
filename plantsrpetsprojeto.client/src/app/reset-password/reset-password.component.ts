import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reset-password',
  standalone: false,
  
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.css'
})
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup;
  token: string = '';
  email: string = '';
  successMessage: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.resetForm = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.token = params['token'] || '';
    });
  }

  resetPassword(): void {
    if (this.resetForm.invalid) return;

    const newPassword = this.resetForm.value.newPassword;
    const confirmPassword = this.resetForm.value.confirmPassword;

    if (newPassword !== confirmPassword) {
      this.errorMessage = "As senhas não coincidem!";
      return;
    }

    this.isLoading = true;

    this.http.post('api/reset-password', {
      email: this.email,
      token: this.token,
      newPassword
    }).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = "Senha redefinida com sucesso! Redirecionando...";
        this.errorMessage = '';

        setTimeout(() => this.router.navigate(['/']), 2000);
      },
      error: err => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || "Ocorreu um erro. Tente novamente.";
        this.successMessage = '';
      }
    });
  }
}
