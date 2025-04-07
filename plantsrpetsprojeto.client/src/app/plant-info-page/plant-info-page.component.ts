import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TipService } from '../tips.service';
import { Tip } from '../models/tip-model';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';
import { Location } from '@angular/common';

@Component({
  selector: 'app-plant-info-page',
  standalone: false,
  templateUrl: './plant-info-page.component.html',
  styleUrls: ['./plant-info-page.component.css']
})

/**
 * Componente responsável por apresentar os detalhes de uma planta específica,
 * incluindo informações como nome, tipo, características e dicas de sustentabilidade associadas.
 * É carregado com base no `id` fornecido na rota.
 */
export class PlantInfoPageComponent implements OnInit {

  id!: string;
  tips: Tip[] = [];
  plant: PlantInfo | null = null;
  isLoading = true;
  errorMessage: string | null = null;

  /**
   * Injeta os serviços necessários:
   * @param route Serviço de rota para aceder aos parâmetros da URL
   * @param tipService Serviço para obter dicas de sustentabilidade
   * @param plantsService Serviço para obter informações da planta
   * @param location Serviço para navegar de volta (equivalente ao botão "voltar")
   */
  constructor(
    private route: ActivatedRoute,
    private tipService: TipService,
    private plantsService: PlantsService,
    private location: Location
  ) { }

  /**
   * Lifecycle hook chamado ao iniciar o componente.
   * Responsável por:
   *  - obter o ID da planta da rota;
   *  - carregar os dados da planta;
   *  - carregar as dicas associadas.
   */
  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id')!;

    if (!this.id) {
      this.errorMessage = 'Invalid plant ID';
      this.isLoading = false;
      return;
    }

    this.plantsService.getPlantById(+this.id).subscribe({
      next: (plant) => {
        this.plant = plant;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load plant details';
        this.isLoading = false;
        console.error(err);
      }
    });

    this.fetchTips();
  }

  /**
   * Recolhe as dicas de sustentabilidade relacionadas com a planta atual.
   */
  private fetchTips(): void {
    this.tipService.getTipsByPlantId(+this.id).subscribe({
      next: (data) => {
        this.tips = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load tips. Please try again later.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Retorna o nome científico da planta como uma única string.
   * @returns Nome científico formatado ou string vazia se não existir
   */
  getScientificName(): string {
    return this.plant?.scientificName?.join(' ') || '';
  }

  /**
   * Ação de retroceder à página anterior no histórico de navegação.
   */
  goBack() {
    this.location.back();
  }

}
