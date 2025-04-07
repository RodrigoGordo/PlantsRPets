import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { UserProfile } from '../models/user-profile';
import { Pet } from '../models/pet';
import { HttpClient } from '@angular/common/http';
import { CollectionService } from '../collections.service';
import { Plantation } from '../models/plantation.model';
import { PlantationsService } from '../plantations.service';

@Component({
    standalone: false,
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})

/**
 * Componente responsável por apresentar e editar o perfil do utilizador.
 * Permite visualizar os dados do perfil, editar a biografia e imagem de perfil, 
 * e exibir os pets favoritos e plantações em destaque.
 */
export class ProfileComponent implements OnInit {
  isEditing = false;
  userProfile: UserProfile = {
    nickname: '',
    profile: {
        bio: '',
        profilePicture: null,
        favoritePets: [],
        highlightedPlantations: [],
        profileId: 0,
        userId: ''
    }
  };
  favoritePets: Pet[] = [];
  highlightedPlantations: Plantation[] = []
  loading: boolean = true;
  error: string | null = null;


  /**
   * Construtor do componente, injeta os serviços necessários para carregar dados e gerir o perfil.
   * @param plantationsService Serviço de gestão de plantações do utilizador
   * @param collectionService Serviço responsável pela coleção de pets
   * @param http Serviço HTTP para chamadas diretas à API
   * @param authService Serviço de autenticação e gestão de utilizador
   */
  constructor(private plantationsService: PlantationsService, private collectionService: CollectionService, private http: HttpClient, private authService: AuthorizeService) { }

  /**
   * Lifecycle hook do Angular.
   * Carrega o perfil, pets favoritos e plantações em destaque ao inicializar o componente.
   */
  ngOnInit(): void {
    this.loadProfile();
    this.loadFavoritePets();
    this.loadHighlightedPlantations();
  }

  /**
   * Carrega os dados de perfil do utilizador autenticado a partir do serviço de autenticação.
   */
  loadProfile(): void {
    this.authService.getUserProfile().subscribe(
      (data) => {
          this.userProfile = data;
          console.log("Profile data:", this.userProfile);
          console.log("Profile picture URL:", this.userProfile.profile.profilePicture);
      },
      (error) => {
          console.error('Error loading profile', error);
      }
    );
  }

  /**
   * Alterna entre os modos de visualização e edição do perfil.
   * Se for desligado o modo de edição, tenta guardar as alterações feitas.
   */
  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.saveProfile();
    }
  }

  /**
   * Envia os dados atualizados do perfil para o backend.
   * Inclui a biografia e uma nova imagem de perfil (caso selecionada).
   */
  saveProfile(): void {
    const fileInput = document.getElementById('file-upload') as HTMLInputElement;
    const file = fileInput.files?.[0] || null;

    this.userProfile.profile.bio = this.userProfile.profile.bio || '';

    this.authService.updateProfile(this.userProfile, file).subscribe(
      (response) => {
        console.log('Profile updated successfully', response);
        this.userProfile = response;
      },
      (error) => {
        console.error('Error updating profile', error);
      }
    );
  }

  /**
   * Aciona o clique no input de upload de imagem de perfil.
   */
  triggerFileUpload(): void {
     document.getElementById('file-upload')?.click();
  }

  /**
   * Processa o ficheiro de imagem selecionado para atualização do perfil.
   * Valida tipo e tamanho do ficheiro e aplica pré-visualização.
   * @param event Evento de alteração do input file
   */
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];

      if (!file.type.startsWith('image/')) {
        alert('Por favor, selecione um ficheiro de imagem (JPG, PNG, etc.).');
        input.value = '';
        return;
      }

      const maxSize = 10 * 1024 * 1024;
      if (file.size > maxSize) {
        alert('O ficheiro é muito grande. O tamanho máximo permitido é 10 MB.');
        input.value = '';
        return;
      }

      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.userProfile.profile.profilePicture = e.target.result;
      };
      reader.readAsDataURL(file);
    }
  }

  /**
   * Carrega os pets favoritos do utilizador da coleção, ordenados por nome e se são owned.
   * Limita a 5 entradas.
   */
  loadFavoritePets(): void {
    this.loading = true;
    this.http.get<Pet[]>('/api/collections').subscribe({
      next: (data) => {
        this.favoritePets = data
        .filter(pet => pet.isFavorite)
        .sort((a, b) => {
          if (b.isOwned !== a.isOwned) {
            return Number(b.isOwned) - Number(a.isOwned);
          }
          return a.name.localeCompare(b.name);
        })
        .slice(0, 5);

        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load your collection. Please try again later.';
        this.loading = false;
        console.error('Error loading collection:', err);
      }
    });
  }

  /**
   * Carrega as plantações em destaque do utilizador, ordenadas por nível descendente.
   * Mostra no máximo 3 plantações.
   */
  loadHighlightedPlantations() {
    this.plantationsService.getUserPlantations().subscribe(
      (data: Plantation[]) => {
        console.log(data);
        this.highlightedPlantations = data
          .sort((a, b) => b.level - a.level) 
          .slice(0, 3);
      },
      (error: any) => console.error('Error fetching plantations:', error)
    );
  }
}
