import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface PetDetails {
  petId: number;
  name: string;
  type: string;
  details: string;
  battleStats: string;
  imageUrl: string;
  isOwned: boolean;
  isFavorite: boolean;
}

@Component({
  selector: 'app-pet-details',
  standalone: false,
  templateUrl: './pet-details.component.html',
  styleUrl: './pet-details.component.css'
})
export class PetDetailsComponent implements OnInit {
  pet: PetDetails | null = null;
  loading: boolean = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.error = 'Invalid pet ID';
      this.loading = false;
      return;
    }

    this.http.get<any[]>('/api/collections').subscribe({
      next: (collection) => {
        const petInCollection = collection.find(p => p.petId === +id);

        if (!petInCollection || !petInCollection.isOwned) {
          this.router.navigate(['/collection']);
          return;
        }

        this.http.get<PetDetails>(`/api/pets/${id}`).subscribe({
          next: (data) => {
            this.pet = {
              ...data,
              isOwned: true,
              isFavorite: petInCollection.isFavorite
            };
            this.loading = false;
          },
          error: (err) => {
            this.error = 'Failed to load pet details';
            this.loading = false;
            console.error('Error loading pet details:', err);
          }
        });
      },
      error: (err) => {
        this.error = 'Failed to verify pet ownership';
        this.loading = false;
        console.error('Error verifying pet ownership:', err);
      }
    });
  }

  toggleFavorite(): void {
    if (!this.pet) return;

    this.http.put<{ isFavorite: boolean }>(`/api/collections/favorite/${this.pet.petId}`, {}).subscribe({
      next: (response) => {
        if (this.pet) {
          this.pet.isFavorite = response.isFavorite;
        }
      },
      error: (err) => {
        console.error('Error toggling favorite status:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/collection']);
  }
}
