import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from './authorize.service';
import { jwtDecode } from 'jwt-decode';
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from './signin/signin.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthorizeService, private dialog: MatDialog) { }

  /**
   * Executa lógica de verificação de token no início da aplicação.
   * - Valida se o token JWT expirou e faz logout se necessário.
   * - Controla a abertura automática da janela de login com base na flag 'showLoginPopup' armazenada no localStorage.
   * - Subscreve a eventos de login solicitados através do serviço AuthorizeService.
   */
  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    const popupFlag = localStorage.getItem('showLoginPopup');

    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const exp = decoded.exp * 1000;

        if (Date.now() >= exp) {
          this.authService.signOut();
          localStorage.setItem('showLoginPopup', 'true');

          if (window.location.pathname !== '/') {
            window.location.href = '/';
          }
        }
      } catch (error) {
        this.authService.signOut();
        localStorage.setItem('showLoginPopup', 'true');

        if (window.location.pathname !== '/') {
        window.location.href = '/';
        }
      }
    }

    if (popupFlag === 'true') {
      localStorage.removeItem('showLoginPopup');
      this.dialog.open(SigninComponent, {
        width: '520px',
        panelClass: 'custom-dialog-container',
        disableClose: true
      });
    }

    this.authService.loginRequested$.subscribe(() => {
      this.dialog.open(SigninComponent, {
        width: '520px',
        panelClass: 'custom-dialog-container',
        disableClose: true
      });
    });
  }

}
