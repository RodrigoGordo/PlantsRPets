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
      this.plantationsService.createPlantation(this.plantationForm.value).subscribe(() => {
        this.dialogRef.close(true);
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
