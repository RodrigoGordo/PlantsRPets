import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";

// this will intercept all http requests and redirect to signin if the user is not authenticated and
// is trying to access a protected route
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = localStorage.getItem('authToken'); // Obter token do localStorage
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
            this.router.navigate(['signin']); // Redirecionar para o login
          }
        },
      })
    );
  }
}
