import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthorizeService } from "../authorize.service";
import { MatDialogRef } from '@angular/material/dialog';
import { HttpClient } from "@angular/common/http";


/**
 * Componente responsável pela autenticação de utilizadores na aplicação.
 * Fornece um formulário para login, validando credenciais e gerindo o fluxo
 * de autenticação, incluindo o redirecionamento para a página principal após o login.
 */
@Component({
  selector: 'app-signin-component',
  styleUrl: './signin.component.css',
  templateUrl: './signin.component.html',
  standalone: false,
})
export class SigninComponent {
  loginForm!: FormGroup;
  authFailed: boolean = false;
  signedIn: boolean = false;
  errorMessage: string = '';
  isLoading: boolean = false;
  loginSuccess: boolean = false;

  /**
   * Construtor do componente que inicializa o formulário de login e verifica o estado de autenticação do utilizador.
   * 
   * @param fb - FormBuilder para criar o formulário de login.
   * @param dialogRef - Referência à janela modal atual, permitindo o seu controlo (abrir/fechar).
   * @param authService - Serviço de autenticação responsável pela gestão de tokens e validação de credenciais.
   * @param router - Serviço de navegação do Angular para redirecionamento após login bem-sucedido.
   * @param http - Serviço HTTP para enviar o pedido de signin ao backend.
   */
  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<SigninComponent>, private authService: AuthorizeService, private router: Router, private http: HttpClient) {
    this.signedIn = this.authService.isSignedIn();
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  /**
   * Submete o formulário de login após validação dos campos.
   * Se as credenciais forem válidas, autentica o utilizador e redireciona para a página inicial.
   * Em caso de falha, apresenta uma mensagem de erro.
   */
  onSubmit(): void {
    if (!this.loginForm.valid) return;
    this.isLoading = true;

    const { email, password } = this.loginForm.value;

    this.http.post('api/signin', {
      email,
      password
    }).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response) {
          this.authService.signIn(email, password).subscribe();
          this.loginSuccess = true;
          this.errorMessage = "";
          setTimeout(() => {
            this.dialogRef.close(true);
            this.router.navigateByUrl("/home");
          }, 1300);
        }
      },
      error: err => {
        this.isLoading = false;
        this.authFailed = true;
        this.loginSuccess = false;
        this.errorMessage = err.error?.message;
      }
    });
  }

  /**
   * Fecha o diálogo de login e redireciona o utilizador para a página de recuperação de palavra-passe.
   */
  openForgotPassword(): void {
    this.dialogRef.close();
    this.router.navigate(['/forgot-password']);
  }

  /**
   * Fecha a janela de login sem realizar qualquer ação adicional.
   */
  closeDialog(): void {
    this.dialogRef.close();
  }
}
