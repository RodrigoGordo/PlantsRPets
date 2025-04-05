import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';
import { Plantation } from '../models/plantation.model';
import { tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { RemovePlantPopupComponent } from '../remove-plant-popup/remove-plant-popup.component';

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

  canHarvest: boolean = false;
  harvestCooldownMsg: string = '';
  nextHarvestDate: string = '';
  timeRemainingDays: number = 0;

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
    private dialog: MatDialog,
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
        this.checkHarvestStatus();
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

  openRemoveDialog(): void {
    const dialogRef = this.dialog.open(RemovePlantPopupComponent, {
      width: '420px',
      panelClass: 'custom-dialog-container',
      data: {
        plantName: this.plantationPlant.referencePlant.plantName,
        maxQuantity: this.plantationPlant.quantity,
        plantationId: this.plantationId,
        plantInfoId: this.plantInfoId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.loadPlantationPlantDetails();
      }
    });
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

  private updateHarvestCooldownMsgFromDate(nextHarvestDateStr: string) {
    const now = new Date();
    const nextHarvestDate = new Date(nextHarvestDateStr);
    const diffMs = nextHarvestDate.getTime() - now.getTime();

    if (diffMs <= 0) {
      this.harvestCooldownMsg = '✅ Ready to harvest!';
      return;
    }

    const diffMinutes = Math.floor(diffMs / 60000);
    const days = Math.floor(diffMinutes / (24 * 60));
    const hours = Math.floor((diffMinutes % (24 * 60)) / 60);
    const minutes = diffMinutes % 60;

    const msgParts = [];
    if (days > 0) msgParts.push(`${days}d`);
    if (hours > 0) msgParts.push(`${hours}h`);
    if (minutes > 0) msgParts.push(`${minutes}m`);

    this.harvestCooldownMsg = `⏳ Next harvest available in ${msgParts.join(' ')}`;
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

    let totalHours = Math.floor(remaining);
    let minutes = Math.round((remaining % 1) * 60);

    if (minutes === 60) {
      totalHours += 1;
      minutes = 0;
    }

    const days = Math.floor(totalHours / 24);
    const hours = totalHours % 24;

    this.remainingCooldown = `${days}d ${hours}h ${minutes}m remaining`;
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

  harvestPlant() {
    if (!this.canHarvest) {
      alert('Plant is not ready for harvest yet!');
      return;
    }

    this.plantationsService.harvestPlant(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (res) => {
          this.loadPlantationPlantDetails();
          this.increaseExperience(true);
        },
        error: (err) => {
          console.error('Harvest failed:', err);
          alert('Failed to harvest plant.');
        }
      });
  }

  checkHarvestStatus() {
    this.plantationsService.checkHarvest(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (res) => {
          this.canHarvest = res.canHarvest;
          this.nextHarvestDate = res.nextHarvestDate;

          if (!res.canHarvest && res.nextHarvestDate) {
            this.updateHarvestCooldownMsgFromDate(res.nextHarvestDate);
          } else {
            this.harvestCooldownMsg = '✅ Ready to harvest!';
          }
        },
        error: (err) => {
          console.error('Error checking harvest status:', err);
          this.harvestCooldownMsg = '';
        }
      });
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
        next: () => {
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
