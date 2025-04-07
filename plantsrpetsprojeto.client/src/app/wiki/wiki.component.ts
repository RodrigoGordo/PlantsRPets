import { Component, HostListener, OnInit } from '@angular/core';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-wiki',
  standalone: false,
  templateUrl: './wiki.component.html',
  styleUrls: ['./wiki.component.css']
})
export class WikiComponent implements OnInit {
  plants: PlantInfo[] = [];
  searchQuery = '';
  activeFilters: { [key: string]: any } = {};
  showFilters = false;

  displayCount = 16;
  hasMore = true;

  constructor(private plantService: PlantsService) { }

  ngOnInit(): void {
    this.loadInitialPlants();
  }

  private loadInitialPlants(): void {
    this.plantService.getPlants().subscribe(plants => {
      this.plants = plants;
      this.updateDisplayedPlants();
    });
  }

  get filteredPlants(): PlantInfo[] {
    return this.plants.filter(plant =>
      plant.plantName.toLowerCase().includes(this.searchQuery.toLowerCase()) &&
      this.matchesFilters(plant)
    );
  }

  get displayedPlants(): PlantInfo[] {
    return this.filteredPlants.slice(0, this.displayCount);
  }

  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (this.hasMore && this.isNearBottom()) {
      this.loadMorePlants();
    }
  }

  private isNearBottom(): boolean {
    const threshold = 100;
    const position = window.scrollY + window.innerHeight;
    const height = document.body.scrollHeight;
    return position > height - threshold;
  }

  private loadMorePlants(): void {
    if (this.displayCount < this.filteredPlants.length) {
      this.displayCount += 16;
      this.hasMore = this.displayCount < this.filteredPlants.length;
    }
  }

  private updateDisplayedPlants(): void {
    this.displayCount = 16;
    this.hasMore = this.filteredPlants.length > this.displayCount;
  }


  private matchesFilters(plant: PlantInfo): boolean {
    return Object.keys(this.activeFilters).every(key => {
      const filterValue = this.activeFilters[key];
      if (!filterValue) return true;

      const plantKey = key as keyof PlantInfo;
      const plantValue = plant[plantKey];

      if (['edible', 'fruits'].includes(plantKey)) {
        const plantBool = this.convertYesNoToBoolean(plantValue);
        const filterBool = filterValue === 'true';
        return plantBool === filterBool;
      }

      if (typeof plantValue === 'boolean') {
        return plantValue === (filterValue === 'true');
      }

      if (plantKey === 'sunlight') {
        const normalizedFilter = filterValue.toLowerCase()
          .replace(/[-\/]/g, ' ')
          .replace(/\s+/g, ' ')
          .trim();

        const plantSunlight = plant.sunlight || [];
        return plantSunlight.some(s => {
          const normalizedPlant = (s || '')
            .toLowerCase()
            .replace(/[-\/]/g, ' ')
            .replace(/\s+/g, ' ')
            .trim();

          return normalizedPlant === normalizedFilter;
        });
      }

      if (plantValue == null) return false;

      return plantValue.toString().toLowerCase().trim() ===
        filterValue.toString().toLowerCase().trim();
    });
  }

  private convertYesNoToBoolean(value: any): boolean {
    if (typeof value === 'boolean') return value;
    if (typeof value === 'string') {
      return value.toLowerCase() === 'yes';
    }
    return false;
  }

  onFiltersChanged(filters: any): void {
    this.activeFilters = filters;
    this.updateDisplayedPlants();
  }

  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  onFiltersClosed(): void {
    this.showFilters = false;
  }
}
