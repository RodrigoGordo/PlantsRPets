import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-component',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: false,
})
export class RegisterComponent implements OnInit {
  errors: string[] = [];
  registerForm!: FormGroup;
  registerFailed: boolean = false;
  registerSucceeded: boolean = false;
  signedIn: boolean = false;
  isLoading: boolean = false;

  constructor(
    private authService: AuthorizeService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.signedIn = this.authService.isSignedIn();
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null | object => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (!password || !confirmPassword) return null;
    return password.value !== confirmPassword.value ? { passwordMismatch: true } : null;
  }

  public register(): void {
    if (!this.registerForm.valid) return;

    this.isLoading = true;
    this.registerFailed = false;
    this.errors = [];

    const { name, email, password } = this.registerForm.value;

    this.authService.register(name, email, password).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response) {
          this.registerSucceeded = true;
          this.registerForm.reset();
          setTimeout(() => this.registerSucceeded = false, 5000);
          this.router.navigateByUrl("/");
        }
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Full error:', error);
        if (error.status === 500) {
          console.error('Backend error details:', error.error);
        }
        this.registerFailed = true;
        this.handleRegistrationError(error);
      }
    });
  }

  private handleRegistrationError(error: any): void {
    if (error.error) {
      try {
        const errorObj = JSON.parse(error.error);
        if (errorObj?.errors) {
          for (const field in errorObj.errors) {
            if (errorObj.errors.hasOwnProperty(field)) {
              this.errors.push(...errorObj.errors[field]);
            }
          }
        }
      } catch (e) {
        this.errors.push('An unexpected error occurred');
      }
    }
  }

  openLogin(): void {
    this.dialog.open(SigninComponent, {
      width: '520px',
      panelClass: 'auth-dialog'
    });
  }
}
