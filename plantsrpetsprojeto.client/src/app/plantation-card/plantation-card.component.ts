import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PlantationsService } from '../plantations.service';
import { RecentActivityService } from '../recent-activity.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Plantation } from '../models/plantation.model';
import { CityService } from '../city.service';
import { Location } from '../models/location.model';

@Component({
  selector: 'app-plantation-card',
  standalone: false,
  
  templateUrl: './plantation-card.component.html',
  styleUrl: './plantation-card.component.css'
})

export class PlantationCardComponent {
  @Input() plantation!: any;
  @Output() updated = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<void>();

  isEditing: boolean = false;
  newPlantationName: string = '';


  newLocation!: Location;

  citySearchTerm: string = '';
  citySearchResults: any[] = [];
  private searchDebounceTimer: any;

  constructor(private plantationsService: PlantationsService, private recentActivity: RecentActivityService, private cityService: CityService) { }

  enableEdit(): void {
    this.isEditing = true;
    this.newPlantationName = this.plantation.plantationName;
    if (this.plantation.location) {
      this.newLocation = this.plantation.location;
      this.citySearchTerm = `${this.plantation.location.city}, ${this.plantation.location.region}, ${this.plantation.location.country}`;
    }
  }

  saveEdit(): void {
    if (this.plantation.plantationName == this.newPlantationName) {
      console.log('Plantation name is the same, no need to update.');
    } else {
      this.plantationsService.updatePlantationName(this.plantation.plantationId, this.newPlantationName)
        .subscribe(() => {
          this.plantation.plantationName = this.newPlantationName;
          this.isEditing = false;
          this.updated.emit();
        });
    }

    if (this.newLocation == this.plantation.location) {
      console.log('Location name is the same, no need to update.');

    } else {
      this.plantationsService.updateLocation(this.plantation.plantationId, this.newLocation)
        .subscribe(() => {
          this.plantation.location = this.newLocation;
          this.isEditing = false;
          this.updated.emit();
        });
    }

    this.cancelEdit();
  }

  cancelEdit(): void {
    this.isEditing = false;
  }

  remove(): void {
    if (confirm(`Are you sure you want to delete "${this.plantation.plantationName}"?`)) {
      this.plantationsService.deletePlantation(this.plantation.plantationId)
        .subscribe(() => this.deleted.emit());
      this.recentActivity.removePlantation(this.plantation.plantationId);
    }
  }

  onPlantationSelected(): void {
    this.recentActivity.savePlantation(this.plantation.plantationId);
    console.log(this.recentActivity.getRecentPlantations());
  }

  getTotalPlants(): number {
    return this.plantation.plantationPlants.reduce((sum: number, plant: PlantationPlant) =>
      sum + (plant.quantity || 0), 0) || 0;
  }

  onCitySearchInput(): void {
    clearTimeout(this.searchDebounceTimer);
    this.searchDebounceTimer = setTimeout(() => {
      if (this.citySearchTerm.trim()) {
        this.cityService.getCitiesByName(this.citySearchTerm).subscribe({
          next: (cities) => this.citySearchResults = cities,
          error: (err) => {
            console.error('Error fetching cities:', err);
            this.citySearchResults = [];
          }
        });
      } else {
        this.citySearchResults = [];
      }
    }, 1000);
  }

  selectCity(cityData: any): void {
    this.newLocation = {
      locationId: cityData.id,
      city: cityData.name,
      region: cityData.region,
      country: cityData.country,
      latitude: cityData.lat,
      longitude: cityData.lon
    };

    this.citySearchTerm = `${cityData.name}, ${cityData.region}, ${cityData.country}`;
    this.citySearchResults = [];
  }

}


