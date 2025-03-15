import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-add-plant',
  standalone: false,
  
  templateUrl: './add-plant.component.html',
  styleUrl: './add-plant.component.css'
})
export class AddPlantComponent implements OnInit {
  addPlantForm: FormGroup;
  plants: PlantInfo[] = [];
  filteredPlants: PlantInfo[] = [];
  plantFilter = new FormControl('');

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddPlantComponent>,
    private plantationsService: PlantationsService,
    private plantsService: PlantsService,
    @Inject(MAT_DIALOG_DATA) public plantationId: number
  ) {
    this.addPlantForm = this.fb.group({
      plantInfoId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit(): void {
    this.loadPlants();

    this.plantFilter.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? this.filterPlants(value) : this.plants))
    ).subscribe(filtered => this.filteredPlants = filtered);
  }

  loadPlants(): void {
    this.plantsService.getPlants().subscribe({
      next: (data) => {
        this.plants = data.sort((a, b) => a.plantName.localeCompare(b.plantName));
        this.filteredPlants = this.plants;
      },
      error: (error) => {
        console.error('Error loading plants:', error);
      }
    });
  }

  filterPlants(value: string): PlantInfo[] {
    const filterValue = value.toLowerCase();
    return this.plants.filter(plant => plant.plantName.toLowerCase().includes(filterValue));
  }

  displayPlantName(plant?: PlantInfo): string {
    return plant ? plant.plantName : '';
  }

  selectPlant(event: MatAutocompleteSelectedEvent) {
    const selectedPlant = event.option.value as PlantInfo;
    this.addPlantForm.patchValue({
      plantInfoId: selectedPlant.plantInfoId,
    });

    this.plantFilter.setValue(selectedPlant.plantName, { emitEvent: false });
  }

  addPlant(): void {
    if (this.addPlantForm.valid) {
      const requestData = {
        plantInfoId: this.addPlantForm.value.plantInfoId,
        quantity: this.addPlantForm.value.quantity
      };

      this.plantationsService.addPlantToPlantation(this.plantationId, requestData).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error adding plant:', error);
        }
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
