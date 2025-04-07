import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { AuthorizeService } from "./authorize.service";

@Injectable()
  /**
   * Interceptor responsável por adicionar o token JWT às requisições HTTP protegidas.
   * Também trata erros 401 (Unauthorized) removendo o token e acionando a lógica de logout.
   */
export class AuthInterceptor implements HttpInterceptor {
  constructor(private authService: AuthorizeService, private router: Router) { }

  /**
   * Intercepta requisições HTTP e adiciona o cabeçalho Authorization com o token JWT, se disponível.
   * Se a resposta for 401, efetua logout e redireciona para a homepage com um pedido de login.
   */
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('authToken');
    let authReq = req;

    if (token) {
      authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    }

    return next.handle(authReq).pipe(
      tap({
        error: (event) => {
          if (event instanceof HttpErrorResponse && event.status === 401) {
            localStorage.removeItem('authToken');
            this.authService.signOut();

            this.router.navigate(['/']).then(() => {
              setTimeout(() => {
                localStorage.setItem('showLoginPopup', 'true');
              }, 100);
            });
          }
        },
      })
    );
  }
}




