import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantType } from '../models/plant-type.model';

@Component({
  selector: 'app-create-plantation',
  standalone: false,
  
  templateUrl: './create-plantation.component.html',
  styleUrl: './create-plantation.component.css'
})
export class CreatePlantationComponent {
  plantationForm: FormGroup;
  plantTypes = Object.values(PlantType);

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreatePlantationComponent>,
    private plantationsService: PlantationsService
  ) {
    this.plantationForm = this.fb.group({
      plantationName: ['', [Validators.required, Validators.minLength(3)]],
      plantType: ['', Validators.required]
    });
  }

  createPlantation(): void {
    if (this.plantationForm.valid) {
      const requestData = {
        plantationName: this.plantationForm.value.plantationName,
        plantType: this.plantTypes.indexOf(this.plantationForm.value.plantType)
      };

      this.plantationsService.createPlantation(requestData).subscribe({
        next: () => { this.dialogRef.close(true); },
        error: (error) => { console.error("Erro ao criar plantação:", error); }
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
