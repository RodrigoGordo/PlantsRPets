import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Pet } from '../models/pet';

@Component({
  selector: 'app-pet-card',
  standalone: false,
  
  templateUrl: './pet-card.component.html',
  styleUrl: './pet-card.component.css'
})
export class PetCardComponent {
  @Input() pet!: Pet;

  constructor(private http: HttpClient, private router: Router) { }

  viewPet(): void {
    if (this.pet.isOwned) {
      this.router.navigate(['/pet', this.pet.petId]);
    }
  }

  toggleFavorite(event: Event): void {
    event.stopPropagation();

    this.http.put<{ isFavorite: boolean }>(`/api/collections/favorite/${this.pet.petId}`, {}).subscribe({
      next: (response) => {
          this.pet.isFavorite = response.isFavorite;
      },
      error: (err) => {
        console.error('Error toggling favorite status:', err);
      }
    });
  }

}
