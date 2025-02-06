import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthorizeService } from '../authorize.service';

/**
 * Componente responsável pela barra de navegação da aplicação (navbar).
 * Gera dinamicamente a interface da navegação com base no estado de autenticação do utilizador.
 * Também permite a abertura de diálogos modais para funcionalidades como login e logout.
 */
@Component({
  selector: 'app-navbar',
  standalone: false,

  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  isSignedIn: boolean = false;

  /**
   * Construtor do componente que injeta serviços para gestão de autenticação e modais.
   * 
   * @param dialog - Serviço utilizado para abrir diálogos modais, como login e registo.
   * @param authService - Serviço responsável pela verificação do estado de autenticação do utilizador.
   */
  constructor(
    public dialog: MatDialog,
    private authService: AuthorizeService
  ) {
    this.authService.onStateChanged().subscribe((state: boolean) => {
      this.isSignedIn = state;
    });
  }
}
