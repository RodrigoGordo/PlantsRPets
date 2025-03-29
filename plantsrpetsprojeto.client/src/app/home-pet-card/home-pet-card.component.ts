import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-home-pet-card',
  standalone: false,
  
  templateUrl: './home-pet-card.component.html',
  styleUrl: './home-pet-card.component.css'
})
export class HomePetCardComponent {
  @Input() pets: string[] = [];
}
