import { Component, OnInit } from '@angular/core';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';


@Component({
  selector: 'app-wiki',
  standalone: false,

  templateUrl: './wiki.component.html',
  styleUrl: './wiki.component.css'
})
export class WikiComponent implements OnInit {
  plants: PlantInfo[] = [];
  searchQuery = '';
  activeFilters: any = {};
  showFilters = false;

  constructor(private plantService: PlantsService) {

  }

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
    return Object.keys(this.activeFilters).every((key: string) => {
      const filterValue = this.activeFilters[key];
      if (!filterValue) return true;

      const plantKey = key as keyof PlantInfo;

      if (plantKey === 'sunlight') {
        return plant.sunlight.includes(filterValue);
      }

      if (typeof plant[plantKey] === 'boolean') {
        return plant[plantKey] === (filterValue === 'true');
      }

      return plant[plantKey] === filterValue;
    });
  }

  onFiltersChanged(filters: any): void {
    this.activeFilters = filters;
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }
}
