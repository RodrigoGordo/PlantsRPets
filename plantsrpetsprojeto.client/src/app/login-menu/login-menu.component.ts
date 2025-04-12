import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { AuthorizeService } from '../authorize.service';
import { Router } from '@angular/router';
import { LogoutConfirmationComponent } from '../logout-confirmation/logout-confirmation.component';


@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css'],
  standalone: false,
})

/**
 * Componente responsável pelo menu de login da aplicação.
 * Gera a interface para iniciar sessão, terminar sessão e gerir o estado de autenticação do utilizador.
 * Inclui um dropdown para aceder rapidamente às opções de conta.
 */
export class LoginMenuComponent implements OnInit {
  isSignedIn: boolean = false;
  dropdownOpen: boolean = false;
  isLoading: boolean = false;
  profilePicture: string = "";

  /**
   * Construtor do componente que injeta serviços para controlo de diálogos, autenticação e navegação.
   * 
   * @param dialog - Serviço utilizado para abrir modais de login e confirmação de logout.
   * @param authService - Serviço responsável pela verificação e gestão do estado de autenticação do utilizador.
   * @param router - Serviço de navegação do Angular para redirecionamento após o login/logout.
   */
  constructor(
    public dialog: MatDialog,
    private authService: AuthorizeService,
    private router: Router
  ) {
    this.authService.onStateChanged().subscribe((state: boolean) => {
      this.isSignedIn = state;
    });
  }

  ngOnInit(): void {
    this.authService.onStateChanged().subscribe((state: boolean) => {
      this.isSignedIn = state;
      if (this.isSignedIn) {
        this.loadProfilePicture();
      } else {
        this.profilePicture = "assets /default -profile.png";
      }
    });

    this.authService.loginRequested$.subscribe(() => {
      this.openSignInDialog();
    });
  }

  /**
   * Abre o modal de login, permitindo que o utilizador inicie sessão.
   * O modal está configurado para não poder ser fechado sem uma ação explícita (disableClose).
   */
  openSignInDialog(): void {
    this.isLoading = true;
    this.dialog.open(SigninComponent, {
      width: '520px',
      panelClass: 'custom-dialog-container',
      disableClose: true
    });
    this.isLoading = false;
  }

  /**
   * Abre um modal de confirmação para verificar se o utilizador deseja realmente terminar a sessão.
   * Se o utilizador confirmar, chama o método `signOut()` para concluir o processo de logout.
   */
  confirmLogout(): void {
    const dialogRef = this.dialog.open(LogoutConfirmationComponent, {
      width: '350px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.signOut();
      }
    });
  }

  /**
   * Termina a sessão do utilizador, atualiza o estado de autenticação
   * e redireciona o utilizador para a página principal da aplicação.
   */
  signOut(): void {
    if (this.isSignedIn) {
      this.authService.signOut();
      this.isSignedIn = false;
      this.dropdownOpen = false;
      this.router.navigateByUrl("");
    }
  }

  /**
   * Alterna o estado do menu dropdown entre aberto e fechado.
   * Permite ao utilizador visualizar ou ocultar as opções do menu de login.
   */
  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }

  /**
   * Carrega a imagem de perfil do utilizador autenticado.
   */
  loadProfilePicture(): void {
    this.authService.getUserProfile().subscribe(
      (data) => {
        this.profilePicture = data.profile.profilePicture!;
      },
      (error) => {
        console.error('Error loading profile picture', error);
      }
    );
  }

}
