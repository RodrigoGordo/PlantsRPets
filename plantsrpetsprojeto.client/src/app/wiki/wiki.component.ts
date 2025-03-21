import { Component, OnInit } from '@angular/core';
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

  constructor(private plantService: PlantsService) { }

  ngOnInit(): void {
    this.plantService.getPlants().subscribe(plants => {
      this.plants = plants;
    });
  }

  get filteredPlants(): PlantInfo[] {
    return this.plants.filter(plant =>
      plant.plantName.toLowerCase().includes(this.searchQuery.toLowerCase()) &&
      this.matchesFilters(plant)
    );
  }

  private matchesFilters(plant: PlantInfo): boolean {
    return Object.keys(this.activeFilters).every(key => {
      const filterValue = this.activeFilters[key];
      if (!filterValue) return true;

      const plantKey = key as keyof PlantInfo;
      const plantValue = plant[plantKey];

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

      if (typeof plantValue === 'boolean') {
        return plantValue === (filterValue === 'true');
      }

      if (plantValue == null) return false;

      return plantValue.toString().toLowerCase().trim() ===
        filterValue.toString().toLowerCase().trim();
    });
  }

  onFiltersChanged(filters: any): void {
    this.activeFilters = filters;
  }

  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  onFiltersClosed(): void {
    this.showFilters = false;
  }
}
