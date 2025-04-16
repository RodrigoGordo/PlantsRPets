import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Pet } from '../models/pet';

@Component({
  selector: 'app-collection',
  standalone: false,
  templateUrl: './collection.component.html',
  styleUrl: './collection.component.css'
})

/**
 * Componente responsável pela gestão e visualização da coleção de pets do utilizador.
 * Este componente exibe as coleções de pets colecionáveis da aplicação, mostrando quais o utilizador possui.
 */
export class CollectionComponent implements OnInit {
  pets: Pet[] = [];
  loading: boolean = true;
  error: string | null = null;
  favoriteLimitReached: boolean = false;

  /**
  * Construtor que injeta serviços essenciais:
  * 
  * @param http Serviço HTTP para comunicação com a API backend
  * @param router Serviço de navegação para redirecionamentos se necessário
  */
  constructor(private http: HttpClient, private router: Router) { }

  /**
   * Método do ciclo de vida Angular chamado após a inicialização do componente.
   * Inicia o carregamento da coleção do utilizador.
   */
  ngOnInit(): void {
    this.loadUserCollection();
  }

  /**
   * Obtém a coleção do utilizador a partir da API e organiza os pets:
   * - Primeiro exibe os que são owned
   * - Dentro dos owned, exibe primeiro os favoritos
   */
  loadUserCollection(): void {
    this.loading = true;
    this.http.get<Pet[]>('/api/collections').subscribe({
      next: (data) => {
        this.pets = data.sort((a, b) => {
          if (b.isOwned !== a.isOwned) {
            return Number(b.isOwned) - Number(a.isOwned); // Owned vem primeiro
          }
          return Number(b.isFavorite) - Number(a.isFavorite); // Dentro de owned, favoritos primeiro
        });
        this.favoriteLimitReached = this.getFavoriteCount() >= 5;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load your collection. Please try again later.';
        this.loading = false;
        console.error('Error loading collection:', err);
      }
    });
  }

  getFavoriteCount(): number {
    return this.pets.filter(pet => pet.isFavorite).length;
  }

  onFavoriteToggled(updatedPet: Pet): void {
    const index = this.pets.findIndex(p => p.petId === updatedPet.petId);
    if (index !== -1) {
      this.pets[index].isFavorite = updatedPet.isFavorite;
    }

    this.favoriteLimitReached = this.getFavoriteCount() >= 5;
  }

}
