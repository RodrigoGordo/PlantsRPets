import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { UserProfile } from '../models/user-profile';

@Component({
  standalone: false,
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  isEditing = false;
  profile: UserProfile = {
    nickname: '',
    profile: {
      bio: '',
      profilePictureUrl: null, // Use profilePictureUrl
      favoritePets: [],
      highlightedPlantations: [],
      profileId: 0,
      userId: ''
    }
  };

  constructor(private authService: AuthorizeService) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.authService.getUserProfile().subscribe(
      (data) => {
        this.profile = data;
        console.log("Profile data:", this.profile); // Log the data
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
    this.authService.updateProfile(this.profile).subscribe(
      (response) => {
        console.log('Profile updated successfully', response);
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
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.profile.profile.profilePictureUrl = e.target.result; // Use profilePictureUrl
      };
      reader.readAsDataURL(input.files[0]);
    }
  }
}
