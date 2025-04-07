import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-forgot-password',
  standalone: false,
  
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})

/**
 * Componente responsável pelo processo de recuperação de palavra-passe.
 * Permite que o utilizador solicite o envio de um link para redefinir a sua palavra-passe,
 * através do e-mail associado à conta.
 */
export class ForgotPasswordComponent {
  forgotPasswordForm: FormGroup;
  message: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  /**
   * Construtor do componente que inicializa o formulário de recuperação de palavra-passe e gere dependências.
   * 
   * @param fb - Serviço do Angular para a criação e gestão de formulários reativos.
   * @param http - Serviço HTTP utilizado para enviar o pedido de recuperação ao backend.
   */
  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  /**
   * Envia o pedido de recuperação de palavra-passe para o backend.
   * Após validação do formulário, é efetuada uma chamada HTTP para enviar o link de recuperação para o e-mail do utilizador.
   * Em caso de sucesso, uma mensagem é exibida; em caso de erro, é apresentada uma mensagem de erro.
   */
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
