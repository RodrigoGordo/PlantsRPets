import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';
import { Plantation } from '../models/plantation.model';
import { MatDialog } from '@angular/material/dialog';
import { RemovePlantPopupComponent } from '../remove-plant-popup/remove-plant-popup.component';

@Component({
  standalone: false,
  selector: 'app-plantation-plant-details',
  templateUrl: './plantation-plant-details.component.html',
  styleUrls: ['./plantation-plant-details.component.css']
})

  /**
   * Componente responsável por exibir os detalhes de uma planta específica numa plantação.
   * Permite ao utilizador visualizar o estado da planta, regá-la ou realizar a colheita,
   * além de controlar os intervalos de cooldown entre essas ações.
   */
export class PlantationPlantDetailsComponent implements OnInit, OnDestroy {
  plantInfoId!: number;
  plantationId!: number;

  plantationPlant!: PlantationPlant;
  plantation!: Plantation;

  canHarvest: boolean = false;
  harvestCooldownMsg: string = '';

  canWater: boolean = false;
  waterCooldownMsg: string = '';

  isLoading: boolean = false;
  errorMessage: string = '';

  private updateInterval!: any;

  /**
 * Injeta os serviços necessários para navegação, gestão de plantações,
 * rotas ativas e diálogos modais.
 *
 * @param dialog Serviço de abertura de diálogos (modais).
 * @param route Serviço de acesso aos parâmetros da rota.
 * @param plantationsService Serviço para manipulação de dados de plantações.
 * @param location Serviço que permite voltar à página anterior.
 */
  constructor(
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private plantationsService: PlantationsService,
    private location: Location
  ) { }

  /**
   * Lifecycle hook que é executado quando o componente é inicializado.
   * Lê os parâmetros da rota para obter o ID da plantação e da planta,
   * e inicia o carregamento dos dados correspondentes.
   */
  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.plantationId = Number(params.get('plantationId'));
      this.plantInfoId = Number(params.get('plantInfoId'));
      this.loadPlantationPlantDetails();
    });

    // Atualiza o status a cada minuto
    this.updateInterval = setInterval(() => {
      this.updateStatus();
    }, 60000);
  }

  /**
   * Lifecycle hook que é chamado ao destruir o componente.
   * Limpa o intervalo de atualização do cooldown para evitar memory leaks.
   */
  ngOnDestroy() {
    clearInterval(this.updateInterval);
  }

  /**
   * Carrega os dados de uma planta específica associada a uma plantação.
   * Atualiza também os estados de cooldown e colheita.
   */
  loadPlantationPlantDetails() {
    if (!this.plantationId || !this.plantInfoId) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.plantationsService.getPlantationPlantById(this.plantationId, this.plantInfoId).subscribe({
      next: (data) => {
        this.plantationPlant = data;
        this.updateStatus();
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Erro ao carregar detalhes da planta';
      }
    });
    this.plantationsService.getPlantationById(this.plantationId).subscribe({
      next: (plantation) => {
        this.plantation = plantation;
      },
      error: (err) => {
        console.error('Erro ao carregar plantação:', err);
      }
    });
  }

  /**
   * Atualiza o status de rega e colheita da planta.
   */
  private updateStatus() {
    this.checkWaterStatus();
    this.checkHarvestStatus();
  }

  /**
   * Verifica o status de rega da planta através do serviço.
   */
  private checkWaterStatus() {
    if (!this.plantationPlant) return;

    this.plantationsService.checkWater(this.plantationId, this.plantInfoId).subscribe({
      next: (status) => {
        this.canWater = status.canWater;
        this.updateCooldownMessage(
          status.timeRemainingMinutes * 60000,
          'water'
        );
      },
      error: (err) => {
        console.error('Erro ao verificar status de rega:', err);
      }
    });
  }

  /**
   * Verifica o status de colheita da planta através do serviço.
   */
  private checkHarvestStatus() {
    this.plantationsService.checkHarvest(this.plantationId, this.plantInfoId).subscribe({
      next: (res) => {
        this.canHarvest = res.canHarvest;
        if (!res.canHarvest && res.nextHarvestDate) {
          const diffMs = new Date(res.nextHarvestDate).getTime() - new Date().getTime();
          this.updateCooldownMessage(diffMs, 'harvest');
        } else {
          this.harvestCooldownMsg = '✅ Pronta para colher!';
        }
      },
      error: (err) => {
        console.error('Erro ao verificar status de colheita:', err);
        this.harvestCooldownMsg = '';
      }
    });
  }

  /**
   * Atualiza a mensagem de cooldown para rega ou colheita.
   * Mostra mensagem específica quando a ação está disponível.
   * @param diffMs Tempo restante em milissegundos
   * @param type Tipo de cooldown ('water' ou 'harvest')
   */
  private updateCooldownMessage(diffMs: number, type: 'water' | 'harvest') {
    // Se o tempo restante for menor ou igual a zero, mostra mensagem de disponível
    if (diffMs <= 0) {
      if (type === 'water') {
        this.waterCooldownMsg = '✅ Pronta para regar!';
      } else {
        this.harvestCooldownMsg = '✅ Pronta para colher!';
      }
      return;
    }

    // Calcula o tempo restante formatado
    const diffMinutes = Math.floor(diffMs / 60000);
    const days = Math.floor(diffMinutes / (24 * 60));
    const hours = Math.floor((diffMinutes % (24 * 60)) / 60);
    const minutes = diffMinutes % 60;

    const msgParts = [];
    if (days > 0) msgParts.push(`${days}d`);
    if (hours > 0) msgParts.push(`${hours}h`);
    if (minutes > 0) msgParts.push(`${minutes}m`);

    const action = type === 'water' ? 'rega' : 'colheita';
    const message = `⏳ Próxima ${action} disponível em ${msgParts.join(' ')}`;

    if (type === 'water') {
      this.waterCooldownMsg = message;
    } else {
      this.harvestCooldownMsg = message;
    }
  }

  /**
   * Abre o diálogo de confirmação para remover a planta da plantação.
   */
  openRemoveDialog(): void {
    const dialogRef = this.dialog.open(RemovePlantPopupComponent, {
      width: '420px',
      panelClass: 'custom-dialog-container',
      data: {
        plantName: this.plantationPlant.referencePlant.plantName,
        maxQuantity: this.plantationPlant.quantity,
        plantationId: this.plantationId,
        plantInfoId: this.plantInfoId
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.loadPlantationPlantDetails();
      }
    });
  }

  /**
   * Realiza a ação de regar a planta.
   * Verifica o cooldown antes de executar.
   */
  waterPlants() {
    if (!this.canWater) {
      alert('A planta ainda está em cooldown!');
      return;
    }

    this.plantationsService.waterPlant(this.plantationId, this.plantInfoId).subscribe({
      next: (updatedPlant) => {
        this.plantationPlant.lastWatered = updatedPlant.lastWatered;
        this.checkWaterStatus();
        this.increaseExperience(false);
      },
      error: (error) => {
        console.error("Falha ao regar", error);
        this.errorMessage = "Erro ao regar a planta";
      }
    });
  }

  /**
   * Realiza a ação de colher a planta.
   * Verifica se está pronta para colheita antes de executar.
   */
  harvestPlant() {
    if (!this.canHarvest) {
      alert('A planta ainda não está pronta para colheita!');
      return;
    }

    this.plantationsService.harvestPlant(this.plantationId, this.plantInfoId).subscribe({
      next: () => {
        this.loadPlantationPlantDetails();
        this.increaseExperience(true);
      },
      error: (err) => {
        console.error('Falha na colheita:', err);
        this.errorMessage = 'Erro ao colher a planta';
      }
    });
  }

  /**
   * Aumenta a experiência da plantação com base na ação realizada.
   * @param isHarvesting Indica se a ação foi colheita (true) ou rega (false)
   */
  increaseExperience(isHarvesting: boolean) {
    this.plantationsService.gainExperience(this.plantationId, this.plantInfoId, isHarvesting).subscribe({
      error: (error) => {
        console.error("Falha ao aumentar experiência", error);
      }
    });
  }

  /**
   * Volta para a página anterior.
   */
  goBack() {
    this.location.back();
  }
}
