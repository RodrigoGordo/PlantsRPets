import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantTypesService } from '../plant-type.service';
import { PlantType } from '../models/plant-type.model';
import { Location } from '../models/location.model';

@Component({
  selector: 'app-create-plantation',
  standalone: false,
  
  templateUrl: './create-plantation.component.html',
  styleUrl: './create-plantation.component.css'
})
export class CreatePlantationComponent {
  plantationForm: FormGroup;
  plantTypes: PlantType[] = [];
  newLocation!: Location;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreatePlantationComponent>,
    private plantationsService: PlantationsService,
    private plantTypesService: PlantTypesService
  ) {
    this.plantationForm = this.fb.group({
      plantationName: ['', [Validators.required, Validators.minLength(3)]], 
      plantTypeId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadPlantTypes();
  }

  loadPlantTypes(): void {
    this.plantTypesService.getPlantTypes().subscribe({
      next: (data) => {
        console.log("Dados recebidos do backend:", data);
        this.plantTypes = data.filter(
          (type, index, self) =>
            index === self.findIndex((t) => t.plantTypeId === type.plantTypeId)
        );
      },
      error: (error) => console.error("Erro ao carregar os tipos de plantas:", error)
    });
  }

  createPlantation(): void {
    if (this.plantationForm.valid) {
      const requestData = {
        plantationName: this.plantationForm.value.plantationName,
        plantTypeId: this.plantationForm.value.plantTypeId,
        location: this.newLocation
      };

      this.plantationsService.createPlantation(requestData).subscribe({
        next: () => { this.dialogRef.close(true); },
        error: (error) => { console.error("Erro ao criar plantação:", error); }
      });
    }
  }

  onLocationSelected(location: Location): void {
    this.newLocation = location;
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
