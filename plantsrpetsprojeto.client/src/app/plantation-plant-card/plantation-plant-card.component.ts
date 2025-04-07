import { Component, Input } from '@angular/core';
import { PlantInfo } from '../models/plant-info';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-plantation-plant-card',
  standalone: false,
  
  templateUrl: './plantation-plant-card.component.html',
  styleUrl: './plantation-plant-card.component.css'
})

/**
 * Componente responsável por representar visualmente uma planta específica dentro de uma plantação.
 * Permite ao utilizador navegar até à página de detalhes da planta associada à plantação.
 */
export class PlantationPlantCardComponent {
  @Input() plant!: PlantInfo;
  plantationId! : Number;

  /**
   * Injeta os serviços necessários:
   * @param router - Permite navegação programática.
   * @param route - Permite acesso aos parâmetros da rota atual.
   */
  constructor(private router: Router, private route: ActivatedRoute) { }

  /**
   * Ciclo de vida que ocorre após a criação do componente.
   * Responsável por obter o ID da plantação a partir da rota.
   */
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.plantationId = Number(this.route.snapshot.paramMap.get('id'));
      console.log(this.plantationId);
    })

  }

  /**
   * Redireciona o utilizador para a página de detalhes da planta dentro da plantação.
   * O caminho é construído com base nos IDs da plantação e da planta.
   */
  navigateToPlantPlantationDetails() {
    this.router.navigate([`/plantation/${this.plantationId}/plant/${this.plant.plantInfoId}`]);
  }
}
