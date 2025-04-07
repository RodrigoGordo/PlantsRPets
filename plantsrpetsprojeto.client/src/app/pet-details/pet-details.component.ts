import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

/**
 * Interface que define os detalhes de um Pet.
 */
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

/**
 * Componente responsável por exibir os detalhes de um pet pertencente à coleção do utilizador.
 * Verifica se o pet é realmente "owned" antes de carregar os dados e permite marcar como favorito.
 */

export class PetDetailsComponent implements OnInit {
  pet: PetDetails | null = null;
  loading: boolean = true;
  error: string | null = null;

  /**
   * Construtor do componente.
   * 
   * @param route - Serviço de rota usado para obter o ID do pet a partir da URL.
   * @param http - Serviço HTTP usado para fazer pedidos à API.
   * @param router - Serviço de navegação para redirecionar o utilizador caso necessário.
   */
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) { }

  /**
   * Lifecycle hook do Angular executado quando o componente é inicializado.
   * Verifica se o utilizador possui o pet e carrega os detalhes.
   */
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

  /**
   * Alterna o estado de favorito de um pet e atualiza a coleção do utilizador.
   */
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

  /**
   * Navega de volta para a página da coleção.
   */
  goBack(): void {
    this.router.navigate(['/collection']);
  }
}
