import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AddPlantComponent } from '../add-plant/add-plant.component';
import { PlantationsService } from '../plantations.service';
import { Plantation } from '../models/plantation.model';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-plantation-details',
  standalone: false,
  
  templateUrl: './plantation-details.component.html',
  styleUrl: './plantation-details.component.css'
})
export class PlantationDetailsComponent implements OnInit {
  plantation: Plantation = {
    plantationId: 0,
    plantationName: '',
    plantTypeId: 0,
    plantTypeName: '',
    lastWatered: new Date(),
    plantingDate: new Date(),
    harvestDate: new Date(),
    growthStatus: '',
    experiencePoints: 0,
    level: 0,
    plantationPlants: []
  };

  plantationPlants: PlantInfo[] = [];
  loading: boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private plantationsService: PlantationsService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadPlantation();
    this.loadPlantationPlants();
  }

  loadPlantation(): void {
    const plantationId = Number(this.route.snapshot.paramMap.get('id'));
    if (!plantationId) {
      this.errorMessage = "Invalid Plantation ID";
      this.loading = false;
      return;
    }

    this.plantationsService.getPlantationById(plantationId).subscribe({
      next: (data) => {
        this.plantation = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = "Failed to load plantation details.";
        this.loading = false;
      }
    });

  }

  loadPlantationPlants(): void {
    const plantationId = Number(this.route.snapshot.paramMap.get('id'));
    if (!plantationId) return;

    this.plantationsService.getPlantsInPlantation(plantationId).subscribe({
      next: (data) => {
        console.log("Plants received:", data);
        this.plantationPlants = data.map(pp => pp.referencePlant);
      },
      error: () => {
        this.errorMessage = "Failed to load plants.";
      }
    });
  }

  openAddPlantDialog(): void {
    const dialogRef = this.dialog.open(AddPlantComponent, {
      width: '400px',
      data: this.plantation.plantationId
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPlantationPlants();
      }
    });
  }
}
