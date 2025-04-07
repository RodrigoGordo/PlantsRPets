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

/**
 * Componente responsável pela redefinição da palavra-passe dos utilizadores.
 * Permite ao utilizador definir uma nova palavra-passe após receber um link de recuperação por e-mail.
 */
export class ResetPasswordComponent implements OnInit {
  resetForm: FormGroup;
  token: string = '';
  email: string = '';
  successMessage: string = '';
  errorMessage: string = '';
  isLoading: boolean = false;

  /**
   * Construtor do componente que inicializa o formulário de redefinição da palavra-passe
   * e gere as dependências de navegação, formulários e comunicação com o servidor.
   *
   * @param route - Serviço para aceder aos parâmetros da rota (e.g., token e e-mail).
   * @param fb - FormBuilder para criar o formulário de redefinição da palavra-passe.
   * @param http - Serviço HTTP para enviar o pedido de redefinição da palavra-passe ao backend.
   * @param router - Serviço de navegação do Angular para redirecionar o utilizador após o sucesso.
   */
  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.resetForm = this.fb.group({
      newPassword: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/)]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  /**
   * Método do ciclo de vida do Angular chamado na inicialização do componente.
   * Recupera o token e o e-mail dos parâmetros da URL, necessários para validar o pedido de redefinição.
   */
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
      this.token = params['token'] || '';
    });
  }

  /**
   * Processa o pedido de redefinição da palavra-passe após a validação do formulário.
   * Valida se as palavras-passe coincidem e envia um pedido ao servidor para concluir a operação.
   * Apresenta mensagens de sucesso ou erro de acordo com o resultado da operação.
   */
  resetPassword(): void {
    if (this.resetForm.invalid) return;

    const newPassword = this.resetForm.value.newPassword;
    const confirmPassword = this.resetForm.value.confirmPassword;

    if (newPassword !== confirmPassword) {
      this.errorMessage = "The passwords don't match!";
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
        this.successMessage = "Password reset successfully!";
        this.errorMessage = '';

        setTimeout(() => this.router.navigate(['/']), 2000);
      },
      error: err => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || "An error occurred. Please try again.";
        this.successMessage = '';
      }
    });
  }
}
