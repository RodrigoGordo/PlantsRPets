import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { PlantPeriodWarningComponent } from '../plant-period-warning/plant-period-warning.component';

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
    private dialog: MatDialog,
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
    this.addPlantForm.patchValue({ plantInfoId: selectedPlant.plantInfoId });

    this.plantFilter.setValue(selectedPlant.plantName, { emitEvent: false });
  }

  addPlant(): void {
    if (this.addPlantForm.valid) {
      const selectedPlantId = this.addPlantForm.value.plantInfoId;
      const quantity = this.addPlantForm.value.quantity;

      this.plantsService.getPlantingPeriodCheck(selectedPlantId).subscribe({
        next: (data) => {

          if (!data.idealMonths || data.idealMonths.length === 0) {
            this.confirmAddPlant(selectedPlantId, quantity);
            return;
          }

          if (!data.isIdealTime) {
            const confirmDialog = this.dialog.open(PlantPeriodWarningComponent, {
              data: {
                message: 'Not ideal to plant now. Recommended months:',
                idealMonths: data.idealMonths
              }
            });

            confirmDialog.afterClosed().subscribe(confirmed => {
              if (confirmed) {
                this.confirmAddPlant(selectedPlantId, quantity);
              }
            });
          } else {
            this.confirmAddPlant(selectedPlantId, quantity);
          }
        },
        error: (error) => console.error('Error verifying planting time:', error)
      });
    }
  }

  confirmAddPlant(plantInfoId: number, quantity: number): void {
    const requestData = { plantInfoId, quantity };

    this.plantationsService.addPlantToPlantation(this.plantationId, requestData).subscribe({
      next: () => this.dialogRef.close(true),
      error: (error) => console.error('Error adding plant:', error)
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
