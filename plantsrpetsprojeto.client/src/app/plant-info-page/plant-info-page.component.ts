import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-plant-info-page',
  standalone: false,
  templateUrl: './plant-info-page.component.html',
  styleUrls: ['./plant-info-page.component.css']
})
export class PlantInfoPageComponent implements OnInit {
  plant: PlantInfo | null = null;
  isLoading = true;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private plantsService: PlantsService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.errorMessage = 'Invalid plant ID';
      this.isLoading = false;
      return;
    }

    this.plantsService.getPlantById(+id).subscribe({
      next: (plant) => {
        this.plant = plant;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load plant details';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  getScientificName(): string {
    return this.plant?.scientificName?.join(' ') || '';
  }
}
