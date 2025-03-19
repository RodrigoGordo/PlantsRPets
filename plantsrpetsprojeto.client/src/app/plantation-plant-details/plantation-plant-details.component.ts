import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';

type PlantType = 'Tree' | 'Shrub' | 'Vegetable';
type WaterFrequency = 'Minimal' | 'Average' | 'Frequent';

@Component({
  selector: 'app-plantation-plant-details',
  standalone: false,
  templateUrl: './plantation-plant-details.component.html',
  styleUrls: ['./plantation-plant-details.component.css']

})

export class PlantationPlantDetailsComponent implements OnInit {
  plantInfoId!: number;
  plantationId!: number;

  plantationPlant!: PlantationPlant;

  isLoading: boolean = false;
  errorMessage: string = '';

  private cooldownConfig: Record<PlantType, Record<WaterFrequency, number>> = {
    'Tree': { Minimal: 156, Average: 84, Frequent: 60 },
    'Shrub': { Minimal: 144, Average: 84, Frequent: 36 },
    'Vegetable': { Minimal: 48, Average: 36, Frequent: 24 }
  };

  remainingCooldown: string = '';
  private cooldownCheckInterval!: any;

  constructor(
    private route: ActivatedRoute,
    private plantationsService: PlantationsService,
    private location: Location
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.plantationId = Number(this.route.snapshot.paramMap.get('plantationId'));
      this.plantInfoId = Number(this.route.snapshot.paramMap.get('plantInfoId'));
    })

    this.loadPlantationPlantDetails();

    this.cooldownCheckInterval = setInterval(() => {
      this.updateCooldownStatus();
    }, 60000);
  }

  loadPlantationPlantDetails() {
    if (!this.plantationId) return;
    if (!this.plantInfoId) return;

    this.isLoading = true;

    this.plantationsService.getPlantationPlantById(this.plantationId, this.plantInfoId).subscribe({
      next: (data) => {
        console.log("Plant received:", data);
        this.plantationPlant = data;
        this.logPlantData();
        this.updateCooldownStatus();
        this.isLoading = false;
      },
      error: () => {
        console.log("Erro no load das plantas - Msg de erro temp");
        this.isLoading = false;
      }
    });
  }

  ngOnDestroy() {
    clearInterval(this.cooldownCheckInterval);
  }

  get waterFrequency(): string {
    return this.plantationPlant?.referencePlant?.watering;
  }

  get cooldownHours(): number {
    const plantType = this.plantationPlant?.referencePlant?.plantType?.charAt(0).toUpperCase() +
      this.plantationPlant?.referencePlant?.plantType?.slice(1).toLowerCase() as PlantType;
    const wateringFrequency = this.plantationPlant?.referencePlant?.watering as WaterFrequency;

    if (!plantType || !wateringFrequency) {
      console.error('Missing plant type or watering frequency');
      return 0;
    }

    if (!(plantType in this.cooldownConfig)) {
      console.error(`Invalid plant type: ${plantType}`);
      return 0;
    }

    if (!(wateringFrequency in this.cooldownConfig[plantType])) {
      console.error(`Invalid watering frequency: ${wateringFrequency} for ${plantType}`);
      return 0;
    }

    return this.cooldownConfig[plantType][wateringFrequency];
  }

  logPlantData() {
    console.log('Plant Type:', this.plantationPlant?.referencePlant?.plantType);
    console.log('Watering Frequency:', this.plantationPlant?.referencePlant?.watering);
    console.log('Calculated Cooldown:', this.cooldownHours);
  }


  canWater(): boolean {
    if (!this.plantationPlant?.lastWatered) return true;
    const lastWatered = new Date(this.plantationPlant.lastWatered);
    const elapsedHours = (Date.now() - lastWatered.getTime()) / 3600000;
    return elapsedHours >= this.cooldownHours;
  }

  private updateCooldownStatus() {
    if (!this.plantationPlant?.lastWatered) {
      this.remainingCooldown = '';
      return;
    }

    const lastWatered = new Date(this.plantationPlant.lastWatered);
    const elapsedHours = (Date.now() - lastWatered.getTime()) / 3600000;
    const remaining = this.cooldownHours - elapsedHours;

    if (remaining <= 0) {
      this.remainingCooldown = '';
      return;
    }

    const hours = Math.floor(remaining);
    const minutes = Math.round((remaining % 1) * 60);
    this.remainingCooldown = `${hours}h ${minutes}m remaining`;
  }


  waterPlants() {
    if (!this.plantationId) return;
    if (!this.plantInfoId) return;

    if (!this.canWater()) {
      alert('Plant is still in cooldown!');
      return;
    }

    this.plantationsService.waterPlant(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (updatedPlant) => {
          if (this.plantationPlant) {
            this.plantationPlant.lastWatered = updatedPlant.lastWatered;
            this.updateCooldownStatus();
          }

        },
        error: (error) => {
          console.error("Watering Failed", error);
        }
      })
  }

  goBack() {
    this.location.back();
  }
}
