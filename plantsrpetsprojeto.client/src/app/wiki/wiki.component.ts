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

  constructor(private plantService: PlantsService) {

  }

  ngOnInit(): void {
    this.plantService.getPlants().subscribe(plants => {
      this.plants = plants;
    });
  }

  get filteredPlants(): PlantInfo[] {
    return this.plants.filter(plant =>
      plant.plantName.toLowerCase().includes(this.searchQuery.toLowerCase()))
  }
}
