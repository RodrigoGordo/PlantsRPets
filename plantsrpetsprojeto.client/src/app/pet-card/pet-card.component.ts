import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Pet } from '../models/pet';
import { RecentActivityService } from '../recent-activity.service';

@Component({
  selector: 'app-pet-card',
  standalone: false,
  
  templateUrl: './pet-card.component.html',
  styleUrl: './pet-card.component.css'
})

/**
 * Componente responsável por exibir um cartão individual de um pet na coleção.
 * Permite ao utilizador visualizar detalhes do pet (se for "owned") e marcar/desmarcar como favorito.
 */
export class PetCardComponent {
  @Input() pet!: Pet;
  @Output() favorited = new EventEmitter<void>();

  /**
   * Construtor do componente que injeta os serviços necessários.
   * 
   * @param http - Serviço HTTP para comunicações com a API (ex: atualizar estado favorito).
   * @param router - Serviço de navegação do Angular, usado para redirecionar para os detalhes do pet.
   * @param recentActivity - Serviço que regista pets visualizados recentemente pelo utilizador.
   */
  constructor(private http: HttpClient, private router: Router, private recentActivity: RecentActivityService) { }

  /**
   * Abre a página de detalhes do pet, caso o pet pertença ao utilizador (isOwned).
   * Também regista a visualização no histórico recente.
   */
  viewPet(): void {
    if (this.pet.isOwned) {
      this.router.navigate(['/pet', this.pet.petId]);
      this.recentActivity.savePet(String(this.pet.petId))
    }
  }

  /**
   * Alterna o estado de favorito do pet.
   * 
   * @param event - Evento de clique, utilizado para impedir propagação e evitar navegação acidental.
   */
  toggleFavorite(event: Event): void {
    event.stopPropagation();

    this.http.put<{ isFavorite: boolean }>(`/api/collections/favorite/${this.pet.petId}`, {}).subscribe({
      next: (response) => {
        this.pet.isFavorite = response.isFavorite;
        this.favorited.emit();
      },
      error: (err) => {
        console.error('Error toggling favorite status:', err);
      }
    });
  }

}
