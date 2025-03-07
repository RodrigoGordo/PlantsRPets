import { Component, Input } from '@angular/core';
import { PlantInfo } from '../models/plant-info';


@Component({
  selector: 'app-plant-card',
  standalone: false,
  
  templateUrl: './plant-card.component.html',
  styleUrl: './plant-card.component.css'
})
export class PlantCardComponent {
  @Input() plant!: PlantInfo;

}
