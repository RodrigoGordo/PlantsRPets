import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

interface PetItem {
  petId: number;
  name: string;
  type: string;
  details: string;
  battleStats: string;
  imageUrl: string;
  isOwned: boolean;
  isFavorite: boolean;
}

/**
 * Componente responsável pela gestão e visualização da coleção de pets do utilizador.
 * Este componente exibe as coleções de pets colecionáveis da aplicação, mostrando quais o utilizador possui.
 */
@Component({
  selector: 'app-collection',
  standalone: false,
  templateUrl: './collection.component.html',
  styleUrl: './collection.component.css'
})
export class CollectionComponent implements OnInit {
  pets: PetItem[] = [];
  loading: boolean = true;
  error: string | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadUserCollection();
  }

  loadUserCollection(): void {
    this.loading = true;
    this.http.get<PetItem[]>('/api/collections').subscribe({
      next: (data) => {
        this.pets = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load your collection. Please try again later.';
        this.loading = false;
        console.error('Error loading collection:', err);
      }
    });
  }

  viewPet(pet: PetItem): void {
    if (pet.isOwned) {
      this.router.navigate(['/pet', pet.petId]);
    }
  }

  toggleFavorite(event: Event, petId: number): void {
    event.stopPropagation();

    this.http.put<{ isFavorite: boolean }>(`/api/collections/favorite/${petId}`, {}).subscribe({
      next: (response) => {
        const petIndex = this.pets.findIndex(p => p.petId === petId);
        if (petIndex !== -1) {
          this.pets[petIndex].isFavorite = response.isFavorite;
        }
      },
      error: (err) => {
        console.error('Error toggling favorite status:', err);
      }
    });
  }
}
