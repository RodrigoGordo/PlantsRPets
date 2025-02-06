import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { Router } from '@angular/router';


/**
 * Componente responsável pelo registo de novos utilizadores.
 * Permite que os utilizadores criem uma nova conta, validando o formulário e enviando os dados para o backend.
 */
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

  /**
   * Construtor do componente que inicializa serviços essenciais para o registo,
   * como autenticação, criação de formulários, controlo de diálogos e navegação.
   *
   * @param authService - Serviço responsável pela autenticação e registo de utilizadores.
   * @param formBuilder - Serviço para construir e gerir formulários reativos.
   * @param dialog - Serviço para abrir diálogos modais, como o formulário de login.
   * @param router - Serviço de navegação para redirecionar após o registo.
   */
  constructor(
    private authService: AuthorizeService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.signedIn = this.authService.isSignedIn();
  }

  /**
   * Método do ciclo de vida do Angular chamado na inicialização do componente.
   * Inicializa o formulário de registo com as validações necessárias.
   */
  ngOnInit(): void {
    this.initializeForm();
  }

  /**
   * Inicializa o formulário de registo com validações para nome, e-mail, palavra-passe e confirmação da palavra-passe.
   * Inclui um validador personalizado para garantir que as palavras-passe coincidem.
   */
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

  /**
   * Validador personalizado para verificar se a palavra-passe e a confirmação da palavra-passe coincidem.
   *
   * @param control - Grupo de controlos do formulário que contém os campos de palavra-passe.
   * @returns - Um objeto de erro se as palavras-passe não coincidirem, ou null se forem válidas.
   */
  passwordMatchValidator: ValidatorFn = (control: AbstractControl): null | object => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    if (!password || !confirmPassword) return null;
    return password.value !== confirmPassword.value ? { passwordMismatch: true } : null;
  }

  /**
   * Envia o pedido de registo para o backend após validar o formulário.
   * Em caso de sucesso, limpa o formulário e redireciona o utilizador. Caso contrário, apresenta mensagens de erro.
   */
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

  /**
   * Trata os erros de registo, extraindo mensagens de erro do backend para exibir ao utilizador.
   *
   * @param error - Objeto de erro retornado pela API, contendo detalhes sobre a falha.
   */
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
        this.errors.push('An unexpected error occurred!');
      }
    }
  }

  /**
   * Abre o diálogo modal para o formulário de login, permitindo que o utilizador faça login rapidamente após o registo.
   */
  openLogin(): void {
    this.dialog.open(SigninComponent, {
      width: '520px',
      panelClass: 'auth-dialog'
    });
  }
}
