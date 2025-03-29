import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-home-plantation-card',
  standalone: false,
  templateUrl: './home-plantation-card.component.html',
  styleUrl: './home-plantation-card.component.css'
})
export class HomePlantationCardComponent {
  @Input() plantations: string[] = [];
}
