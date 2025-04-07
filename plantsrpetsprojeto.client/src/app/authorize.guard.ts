import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable, map } from "rxjs";
import { AuthorizeService } from "./authorize.service";

@Injectable({ providedIn: 'root' })

/**
 * Guarda de rotas que protege páginas privadas de utilizadores não autenticados.
 * Caso o utilizador não tenha um token válido, é redirecionado para a página de login.
 */
export class AuthGuard implements CanActivate  {
  constructor(private authService: AuthorizeService, private router: Router) { }

  /**
   * Verifica se o utilizador está autenticado antes de permitir o acesso à rota.
   * @returns booleano indicando se a navegação está autorizada
   */
  canActivate(): boolean {
    const isSignedIn = this.authService.isSignedIn();
    if (!isSignedIn) {
      this.router.navigate(['/signin']);
    }
    return isSignedIn;
  }
}
