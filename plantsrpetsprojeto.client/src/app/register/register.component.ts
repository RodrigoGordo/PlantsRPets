import { Component, OnInit } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from "@angular/forms";
import { AuthorizeService } from "../authorize.service";
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { Router } from '@angular/router';
import { HttpClient } from "@angular/common/http";


/**
 * Componente responsável pelo registo de novos utilizadores.
 * Permite que os utilizadores criem uma nova conta, validando o formulário e enviando os dados para o backend.
 */
@Component({
  selector: 'app-register-component',
  styleUrls: ['./register.component.css'],
  templateUrl: './register.component.html',
  standalone: false,
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  registerSucceeded: boolean = false;
  signedIn: boolean = false;
  errorMessage: string = '';
  isLoading: boolean = false;

  /**
   * Construtor do componente que inicializa serviços essenciais para o registo,
   * como autenticação, criação de formulários, controlo de diálogos e navegação.
   *
   * @param authService - Serviço responsável pela autenticação e registo de utilizadores.
   * @param formBuilder - Serviço para construir e gerir formulários reativos.
   * @param dialog - Serviço para abrir diálogos modais, como o formulário de login.
   * @param router - Serviço de navegação para redirecionar após o registo.
   * @param http - Serviço HTTP para enviar o pedido de registo ao backend.
   */
  constructor(
    private authService: AuthorizeService,
    private formBuilder: FormBuilder,
    private dialog: MatDialog,
    private router: Router,
    private http: HttpClient
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
      nickName: ['', Validators.required],
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

    const { nickName, email, password } = this.registerForm.value;

    this.http.post('api/signup', {
      nickName,
      email,
      password
    }).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response) {
          this.registerSucceeded = true;
          this.registerForm.reset();
          this.errorMessage = "";
          setTimeout(() => {
            this.router.navigateByUrl("/");
          }, 1300);
        }
      },
      error: err => {
        this.isLoading = false;
        this.registerSucceeded = false;
        this.errorMessage = err.error?.message;
      }
    });
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
