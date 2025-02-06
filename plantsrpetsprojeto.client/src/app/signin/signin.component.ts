import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthorizeService } from "../authorize.service";
import { MatDialogRef } from '@angular/material/dialog';

/**
 * Componente responsável pela autenticação de utilizadores na aplicação.
 * Fornece um formulário para login, validando credenciais e gerindo o fluxo
 * de autenticação, incluindo o redirecionamento para a página principal após o login.
 */
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
  isLoading: boolean = false;

  /**
   * Construtor do componente que inicializa o formulário de login e verifica o estado de autenticação do utilizador.
   * 
   * @param fb - FormBuilder para criar o formulário de login.
   * @param dialogRef - Referência à janela modal atual, permitindo o seu controlo (abrir/fechar).
   * @param authService - Serviço de autenticação responsável pela gestão de tokens e validação de credenciais.
   * @param router - Serviço de navegação do Angular para redirecionamento após login bem-sucedido.
   */
  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<SigninComponent>, private authService: AuthorizeService, private router: Router) {
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
    if (this.loginForm.valid) {
      this.isLoading = true;
      const { email, password } = this.loginForm.value;

      this.authService.signIn(email, password).subscribe({
        next: (success) => {
          this.isLoading = false; 
          if (success) {
            this.dialogRef.close(true);
            this.router.navigateByUrl("/home");
          }
        },
        error: () => {
          this.isLoading = false;
          this.authFailed = true;
        }
      });
    }
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
