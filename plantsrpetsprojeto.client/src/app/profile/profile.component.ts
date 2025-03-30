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


  constructor(private plantationsService: PlantationsService, private collectionService: CollectionService, private http: HttpClient, private authService: AuthorizeService) { }

    ngOnInit(): void {
      this.loadProfile();
      this.loadFavoritePets();
      this.loadHighlightedPlantations();
    }

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

    toggleEdit(): void {
        this.isEditing = !this.isEditing;
        if (!this.isEditing) {
            this.saveProfile();
        }
    }

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

    triggerFileUpload(): void {
        document.getElementById('file-upload')?.click();
    }

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

  //getFavoritePets(): void {
  //  this.loading = true;
  //  this.collectionService.getFavoritePetsInCollection().subscribe({
  //    next: (data) => {
  //      // Ordenar: owned primeiro, depois favoritos
  //      this.favoritePets = data.sort((a, b) => a.name.localeCompare(b.name));
  //      this.loading = false;
  //    },
  //    error: (err) => {
  //      this.error = 'Failed to load your collection. Please try again later.';
  //      this.loading = false;
  //      console.error('Error loading collection:', err);
  //    }
  //  });
  //}

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
