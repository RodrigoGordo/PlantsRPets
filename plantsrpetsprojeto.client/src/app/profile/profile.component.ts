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
  savingProfile: boolean = false;

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
    this.userProfile = {
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

    this.loadProfile();
    this.loadFavoritePets();
    this.loadHighlightedPlantations();
  }

  /**
   * Carrega os dados de perfil do utilizador autenticado a partir do serviço de autenticação.
   */
  loadProfile(): void {
    this.loading = true;
    this.authService.getUserProfile().subscribe({
      next: (data) => {
        if (data && data.profile) {
          this.userProfile = data;
          console.log("Profile data:", this.userProfile);
          console.log("Profile picture URL:", this.userProfile.profile.profilePicture);
        }
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading profile', error);
        this.loading = false;
      }
    });
  }

  /**
   * Alterna entre os modos de visualização e edição do perfil.
   * Se for desligado o modo de edição, tenta guardar as alterações feitas.
   */
  toggleEdit(): void {
    if (this.isEditing && !this.savingProfile) {
      this.saveProfile();
    }
    this.isEditing = !this.isEditing;
  }

  /**
   * Envia os dados atualizados do perfil para o backend.
   * Inclui a biografia e uma nova imagem de perfil (caso selecionada).
   */
  saveProfile(): void {
    this.savingProfile = true;
    const fileInput = document.getElementById('file-upload') as HTMLInputElement;
    const file = fileInput?.files?.[0] || null;
  
    if (this.userProfile && this.userProfile.profile) {
      this.userProfile.profile.bio = this.userProfile.profile.bio?.slice(0, 100) || '';
    }
  
    const updatedProfile = {
      ...this.userProfile,
      profile: {
        ...this.userProfile.profile,
        bio: this.userProfile.profile?.bio || ''
      }
    };

    this.authService.updateProfile(updatedProfile, file).subscribe({
      next: (response) => {
        console.log('Profile updated successfully', response);
        if (response && response.profile) {
          this.userProfile = response;
        }
        this.savingProfile = false;
      },
      error: (error) => {
        console.error('Error updating profile', error);
        this.savingProfile = false;
      }
    });
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
        alert('O ficheiro deve ter no máximo 10MB.');
        input.value = '';
        return;
      }

      const reader = new FileReader();
      reader.onload = (e: any) => {
        if (this.userProfile && this.userProfile.profile) {
          this.userProfile.profile.profilePicture = e.target.result;
        }
      };
      reader.readAsDataURL(file);
    }
  }

  /**
   * Carrega os pets favoritos do utilizador a partir do serviço de coleção.
   */
  loadFavoritePets(): void {
    this.loading = true;
    this.collectionService.getFavoritePetsInCollection().subscribe({
      next: (pets) => {
        this.favoritePets = pets
          .sort((a, b) => {
            if (b.isOwned !== a.isOwned) {
              return Number(b.isOwned) - Number(a.isOwned);
            }
            return a.name.localeCompare(b.name);
          })
          .slice(0, 5);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading favorite pets', error);
        this.loading = false;
      }
    });
  }

  /**
   * Carrega as plantações em destaque do utilizador a partir do serviço de plantações.
   */
  loadHighlightedPlantations(): void {
    this.plantationsService.getUserPlantations().subscribe({
      next: (plantations: Plantation[]) => {
        this.highlightedPlantations = plantations
          .sort((a, b) => b.level - a.level)
          .slice(0, 3);
      },
      error: (error) => {
        console.error('Error loading highlighted plantations', error);
      }
    });
  }
}
