import { Component, Input } from '@angular/core';
import { PlantInfo } from '../models/plant-info';
import { Router } from '@angular/router';


@Component({
  selector: 'app-plant-card',
  standalone: false,
  
  templateUrl: './plant-card.component.html',
  styleUrl: './plant-card.component.css'
})

/**
 * Componente visual que representa um "cartão" individual de uma planta.
 * Este cartão exibe breves informações sobre uma planta e permite navegar para a sua página de detalhes.
 */
export class PlantCardComponent {
  @Input() plant!: PlantInfo;

  /**
   * Injeta o serviço de roteamento Angular para permitir a navegação programática.
   * 
   * @param router - Serviço de navegação do Angular.
   */
  constructor(private router: Router) { }

  /**
   * Função chamada quando o utilizador interage com o cartão (ex: clica).
   * Redireciona para a página de detalhes da planta com base no seu `plantInfoId`.
   */
  navigateToDetails() {
    this.router.navigate(['/plants', this.plant.plantInfoId]);
  }
}
