import { Component, Input } from '@angular/core';
import { PlantInfo } from '../models/plant-info';
import { Router } from '@angular/router';


@Component({
  selector: 'app-plant-card',
  standalone: false,
  
  templateUrl: './plant-card.component.html',
  styleUrl: './plant-card.component.css'
})
export class PlantCardComponent {
  @Input() plant!: PlantInfo;

  constructor(private router: Router) { }

  navigateToDetails() {
    this.router.navigate(['/plants', this.plant.plantInfoId]);
  }
}
