import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-confirm-email',
  standalone: false,
  
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})

/**
 * Componente responsável por tratar a confirmação de e-mail de um utilizador.
 * Ao ser carregado, lê os parâmetros da URL (email e token) e tenta confirmar o e-mail
 * através do serviço de autenticação. Exibe o resultado ao utilizador.
 */
export class ConfirmEmailComponent implements OnInit {
  message: string = "Verifying your email...";
  success: boolean = false;
  isLoading: boolean = false;

  /**
   * Construtor do componente, responsável por injetar serviços essenciais:
   * 
   * @param route Serviço de rota usado para aceder aos parâmetros da query string (email e token)
   * @param router Serviço de navegação para redirecionar o utilizador após confirmação
   * @param authService Serviço de autenticação com método de confirmação de e-mail
   */
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthorizeService
  ) { }

  /**
   * Método do ciclo de vida chamado após a inicialização do componente.
   * Responsável por extrair os parâmetros da URL e executar o processo de verificação de e-mail.
   */
  ngOnInit(): void {
    this.isLoading = true;

    const email = this.route.snapshot.queryParamMap.get('email');
    const token = this.route.snapshot.queryParamMap.get('token');

    if (email && token) {
      this.authService.confirmEmail(email, token).subscribe(success => {
        if (success) {
          this.isLoading = false;
          this.message = "Your email has been successfully verified!";
          this.success = true;
        } else {
          this.isLoading = false;
          this.message = "Failed to verify email. The link might be invalid or expired.";
          this.success = false;
        }
      });
    } else {
      this.isLoading = false;
      this.message = "Invalid confirmation link.";
    }
  }

  /**
   * Redireciona o utilizador para a página inicial.
   */
  goHome(): void {
    this.router.navigate(['/']);
  }
}
