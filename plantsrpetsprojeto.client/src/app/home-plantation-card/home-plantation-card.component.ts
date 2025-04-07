import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-home-plantation-card',
  standalone: false,
  templateUrl: './home-plantation-card.component.html',
  styleUrl: './home-plantation-card.component.css'
})

/**
 * Componente responsável por exibir plantações destacadas na página inicial.
 * Recebe uma lista de identificadores de plantações como input.
 */
export class HomePlantationCardComponent {
  @Input() plantations: string[] = [];
}
