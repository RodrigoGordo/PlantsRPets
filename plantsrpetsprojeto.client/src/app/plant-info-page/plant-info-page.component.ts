import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TipService } from '../tips.service';
import { Tip } from '../models/tip-model';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';
import { Location } from '@angular/common';

@Component({
  selector: 'app-plant-info-page',
  standalone: false,
  templateUrl: './plant-info-page.component.html',
  styleUrls: ['./plant-info-page.component.css']
})
export class PlantInfoPageComponent implements OnInit {

  id!: string; // Plant ID to fetch tips for
  tips: Tip[] = []; // List of tips
  plant: PlantInfo | null = null;
  isLoading = true;
  errorMessage: string | null = null; // Error message

  constructor(
    private route: ActivatedRoute,
    private tipService: TipService, // Service to fetch tips
    private plantsService: PlantsService,
    private location: Location
  ) { }

  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!;

    if (!this.id) {
      this.errorMessage = 'Invalid plant ID';
      this.isLoading = false;
      return;
    }

    this.plantsService.getPlantById(+this.id).subscribe({
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

    this.fetchTips();
  }

  private fetchTips(): void {
    this.tipService.getTipsByPlantId(+this.id).subscribe({
      next: (data) => {
        this.tips = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load tips. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  getScientificName(): string {
    return this.plant?.scientificName?.join(' ') || '';
  }

  goBack() {
    this.location.back();
  }

}
