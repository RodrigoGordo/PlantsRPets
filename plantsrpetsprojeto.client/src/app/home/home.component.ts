import { Component, OnInit } from '@angular/core';
import { RecentActivityService } from '../recent-activity.service';
import { AuthorizeService } from '../authorize.service';
import { filter } from 'rxjs/operators';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

  /**
   * Componente principal da página inicial.
   * É responsável por carregar e exibir pets e plantações recentemente visualizados pelo utilizador.
   */
export class HomeComponent implements OnInit {
  recentPets: string[] = [];
  recentPlantations: string[] = [];

  /**
   * Injeta os serviços necessários:
   * - `RecentActivityService`: Para aceder às interações recentes do utilizador.
   * - `AuthorizeService`: Para obter informações sobre o utilizador autenticado.
   */
  constructor(
    private recentActivityService: RecentActivityService,
    private authService: AuthorizeService
  ) { }

  /**
   * Lifecycle hook chamado na inicialização do componente.
   * Aguarda pela confirmação de que o perfil do utilizador foi carregado antes de tentar obter os itens recentes.
   */
  ngOnInit(): void {
    this.authService.getUserProfile()
      .pipe(filter(profile => !!profile.profile.userId))
      .subscribe(() => {
        this.loadRecentItems();
      });
  }

  /**
   * Carrega os pets e plantações recentemente visualizados a partir do serviço de atividade recente.
   */
  private loadRecentItems(): void {
    this.recentPets = this.recentActivityService.getRecentPets();
    this.recentPlantations = this.recentActivityService.getRecentPlantations();
  }
}
