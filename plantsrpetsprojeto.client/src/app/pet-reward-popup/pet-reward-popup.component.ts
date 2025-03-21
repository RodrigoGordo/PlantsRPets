import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

interface PetItem {
  petId: number;
  name: string;
  type: string;
  details: string;
  battleStats: string;
  imageUrl: string;
}

@Component({
  selector: 'app-pet-reward-popup',
  standalone: false,
  
  templateUrl: './pet-reward-popup.component.html',
  styleUrl: './pet-reward-popup.component.css'
})

export class PetRewardPopupComponent {
  pets: PetItem[] = [];
  selectedPetId: number | null = null;
  confirming: boolean = false;

  constructor(
    private http: HttpClient,
    private dialogRef: MatDialogRef<PetRewardPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.pets = data.pets;
  }

  selectPet(petId: number): void {
    this.selectedPetId = petId;
  }

  confirmSelection(): void {
    if (this.selectedPetId === null) return;

    this.confirming = true;
    this.http.put(`/api/collections/owned/${this.selectedPetId}`, { isOwned: true }).subscribe({
      next: () => {
        this.confirming = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        console.error('Error claiming pet reward:', err);
        this.confirming = false;
      }
    });
  }

}
