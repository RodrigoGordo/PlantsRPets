import { Component, Input } from '@angular/core';
import { PlantInfo } from '../models/plant-info';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-plantation-plant-card',
  standalone: false,
  
  templateUrl: './plantation-plant-card.component.html',
  styleUrl: './plantation-plant-card.component.css'
})
export class PlantationPlantCardComponent {
  @Input() plant!: PlantInfo;
  plantationId! : Number;

  constructor(private router: Router,
              private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.plantationId = Number(this.route.snapshot.paramMap.get('id'));
      console.log(this.plantationId);
    })

  }

  navigateToPlantPlantationDetails() {
    this.router.navigate([`/plantation/${this.plantationId}/plant/${this.plant.plantInfoId}`]);
    
  }
}
