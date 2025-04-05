import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlantationsService } from '../plantations.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-remove-plant-popup',
  standalone: false,
  
  templateUrl: './remove-plant-popup.component.html',
  styleUrl: './remove-plant-popup.component.css'
})

export class RemovePlantPopupComponent {
  _quantityToRemove: number = 1;
  inputValue: string = '1';
  confirming: boolean = false;
  isSecondConfirmStep = false;

  constructor(
    private plantationsService: PlantationsService,
    private dialogRef: MatDialogRef<RemovePlantPopupComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: {
      plantName: string;
      maxQuantity: number;
      plantationId: number;
      plantInfoId: number;
    }
  ) { }

  get quantityToRemove(): number {
    const value = parseInt(this.inputValue, 10);
    return isNaN(value) ? 0 : Math.min(value, this.data.maxQuantity);
  }

  set quantityToRemove(value: number) {
    if (!value || value < 1) {
      this._quantityToRemove = 1;
    } else if (value > this.data.maxQuantity) {
      this._quantityToRemove = this.data.maxQuantity;
    } else {
      this._quantityToRemove = value;
    }
  }

  onInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.inputValue = input.value;
  }

  onInputBlur(): void {
    const value = parseInt(this.inputValue, 10);

    if (isNaN(value) || value < 1) {
      this.inputValue = '1';
    } else if (value > this.data.maxQuantity) {
      this.inputValue = this.data.maxQuantity.toString();
    }
  }

  confirmRemoval(): void {
    if (this.quantityToRemove === this.data.maxQuantity && !this.isSecondConfirmStep) {
      this.isSecondConfirmStep = true;
      return;
    }

    this.confirming = true;
    this.plantationsService
      .removePlantFromPlantation(this.data.plantationId, this.data.plantInfoId, this.quantityToRemove)
      .subscribe({
        next: () => {
          this.confirming = false;

          if (this.quantityToRemove === this.data.maxQuantity) {
            this.dialogRef.close(true);
            this.router.navigate(['/plantation', this.data.plantationId]);
          } else {
            this.dialogRef.close(true);
          }
        },
        error: (err) => {
          console.error('Error removing plant:', err);
          this.confirming = false;
        }
      });
  }

  cancelConfirmation(): void {
    this.isSecondConfirmStep = true;
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
