import { Component } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})

/**
 * Componente responsável pela página inicial (landing page) da aplicação.
 * Permite a navegação para outras páginas, como a página de recuperação de palavra-passe.
 */
export class LandingPageComponent {

  /**
   * Construtor do componente que injeta o serviço de navegação do Angular.
   * 
   * @param router - Serviço de navegação utilizado para redirecionar o utilizador entre páginas da aplicação.
   */
  constructor(private router: Router) { }

  /**
  * Redireciona o utilizador para a página de recuperação de palavra-passe.
  * Esta funcionalidade é útil para utilizadores que esqueceram as suas credenciais de acesso.
  */
  goToForgotPassword(): void {
    this.router.navigate(['/forgot-password']);
  }
}
