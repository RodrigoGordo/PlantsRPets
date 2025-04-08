import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CityService } from '../city.service';
import { Location } from '../models/location.model';

@Component({
  selector: 'app-location-input',
  standalone: false,
  
  templateUrl: './location-input.component.html',
  styleUrl: './location-input.component.css'
})
export class LocationInputComponent {
  @Output() locationSelected = new EventEmitter<Location>();

  citySearchTerm: string = '';
  citySearchResults: any[] = [];
  private searchDebounceTimer: any;


  constructor(private cityService: CityService) { }

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
    const newLocation: Location = {
      city: cityData.name,
      region: cityData.region,
      country: cityData.country,
      latitude: cityData.lat,
      longitude: cityData.lon
    };

    this.locationSelected.emit(newLocation);
    this.citySearchTerm = `${cityData.name}, ${cityData.region}, ${cityData.country}`;
    this.citySearchResults = [];
  }
}
