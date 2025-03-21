import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';
import { Plantation } from '../models/plantation.model';
import { tap } from 'rxjs';

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
  plantation!: Plantation;

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

    console.log(this.plantationId);

    this.loadPlantationPlantDetails();

    this.plantationsService.getPlantationById(this.plantationId).subscribe({
      next: (plantation) => {
        this.plantation = plantation;
        console.log('Plantation loaded:', this.plantation);
      },
      error: (err) => {
        console.error('Error fetching plantation:', err);
      }
    });

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

    let hours = Math.floor(remaining);
    let minutes = Math.round((remaining % 1) * 60);
    if (minutes == 60) {
      hours += 1;
      minutes = 0;
    }
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
            this.increaseExperience(false);
          }

        },
        error: (error) => {
          console.error("Watering Failed", error);
        }
      })
  }

  get experienceAmount() {
    return 100;
  }

  increaseExperience(isHarvesting: boolean) {
    if (!this.plantation) {
      console.error(`No plantation${this.plantation}`);
    }
    if (!this.plantationPlant) {
      console.error(`No plantationPlant${this.plantationPlant}`);
    }

    this.plantationsService.gainExperience(this.plantationId, this.plantInfoId, isHarvesting)
      .subscribe({
        next: (updatedPlantation) => {
          if (this.plantation) {
            this.plantation.experiencePoints = updatedPlantation.experiencePoints;
          }
        },
        error: (error) => {
          console.error("Experience Increase Failed", error);
        }
      })
  }

  goBack() {
    this.location.back();
  }
}
