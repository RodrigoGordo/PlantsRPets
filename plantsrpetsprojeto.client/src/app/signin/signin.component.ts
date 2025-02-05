import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthorizeService } from "../authorize.service";
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-signin-component',
  templateUrl: './signin.component.html',
  styleUrl: './signin.component.css',
  standalone: false,
})
export class SigninComponent{
  loginForm!: FormGroup;
  authFailed: boolean = false;
  signedIn: boolean = false;

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<SigninComponent>, private authService: AuthorizeService, private router: Router) {
    this.signedIn = this.authService.isSignedIn();
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      this.authService.signIn(email, password).subscribe({
        next: (success) => {
          if (success) {
            this.dialogRef.close(true);
            this.router.navigateByUrl("/home");
          }
        },
        error: () => {
          this.authFailed = true;
        }
      });
    }
  }

  openForgotPassword(): void {
    this.dialogRef.close();
    this.router.navigate(['/forgot-password']);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
