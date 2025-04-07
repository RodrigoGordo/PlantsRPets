import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-home-pet-card',
  standalone: false,
  
  templateUrl: './home-pet-card.component.html',
  styleUrl: './home-pet-card.component.css'
})

/**
 * Componente responsável por exibir os cartões dos pets recentes na página inicial.
 * Recebe uma lista de identificadores de pets como input.
 */
export class HomePetCardComponent {
  @Input() pets: string[] = [];
}
