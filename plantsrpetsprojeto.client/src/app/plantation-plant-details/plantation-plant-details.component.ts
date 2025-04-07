import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';
import { Plantation } from '../models/plantation.model';
import { tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { RemovePlantPopupComponent } from '../remove-plant-popup/remove-plant-popup.component';

type PlantType = 'Tree' | 'Shrub' | 'Vegetable';
type WaterFrequency = 'Minimal' | 'Average' | 'Frequent';

@Component({
  selector: 'app-plantation-plant-details',
  standalone: false,
  templateUrl: './plantation-plant-details.component.html',
  styleUrls: ['./plantation-plant-details.component.css']

})

  /**
   * Componente responsável por exibir os detalhes de uma planta específica numa plantação.
   * Permite ao utilizador visualizar o estado da planta, regá-la ou realizar a colheita,
   * além de controlar os intervalos de cooldown entre essas ações.
   */
export class PlantationPlantDetailsComponent implements OnInit {
  plantInfoId!: number;
  plantationId!: number;

  plantationPlant!: PlantationPlant;
  plantation!: Plantation;

  canHarvest: boolean = false;
  harvestCooldownMsg: string = '';
  nextHarvestDate: string = '';
  timeRemainingDays: number = 0;

  isLoading: boolean = false;
  errorMessage: string = '';

  remainingCooldown: string = '';
  private cooldownCheckInterval!: any;

  /**
   * Configuração dos tempos de cooldown entre regas, dependendo do tipo e frequência.
   */
  private cooldownConfig: Record<PlantType, Record<WaterFrequency, number>> = {
    'Tree': { Minimal: 156, Average: 84, Frequent: 60 },
    'Shrub': { Minimal: 144, Average: 84, Frequent: 36 },
    'Vegetable': { Minimal: 48, Average: 36, Frequent: 24 }
  };

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
      this.plantationId = Number(this.route.snapshot.paramMap.get('plantationId'));
      this.plantInfoId = Number(this.route.snapshot.paramMap.get('plantInfoId'));
    })

    console.log(this.plantationId);

    this.loadPlantationPlantDetails();

    this.plantationsService.getPlantationById(this.plantationId).subscribe({
      next: (plantation) => {
        this.plantation = plantation;
        console.log('Plantation loaded:', this.plantation);
      },
      error: (err) => {
        console.error('Error fetching plantation:', err);
      }
    });

    this.cooldownCheckInterval = setInterval(() => {
      this.updateCooldownStatus();
    }, 60000);
  }

  /**
   * Lifecycle hook que é chamado ao destruir o componente.
   * Limpa o intervalo de atualização do cooldown para evitar memory leaks.
   */
  ngOnDestroy() {
    clearInterval(this.cooldownCheckInterval);
  }

  /**
   * Carrega os dados de uma planta específica associada a uma plantação.
   * Atualiza também os estados de cooldown e colheita.
   */
  loadPlantationPlantDetails() {
    if (!this.plantationId) return;
    if (!this.plantInfoId) return;

    this.isLoading = true;

    this.plantationsService.getPlantationPlantById(this.plantationId, this.plantInfoId).subscribe({
      next: (data) => {
        console.log("Plant received:", data);
        this.plantationPlant = data;
        this.logPlantData();
        this.updateCooldownStatus();
        this.checkHarvestStatus();
        this.isLoading = false;
      },
      error: () => {
        console.log("Erro no load das plantas - Msg de erro temp");
        this.isLoading = false;
      }
    });
  }

  /**
   * Abre a caixa de diálogo para confirmar a remoção da planta da plantação.
   * Se a remoção for confirmada, recarrega os dados da planta.
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
   * Retorna a frequência de rega definida para a planta.
   */
  get waterFrequency(): string {
    return this.plantationPlant?.referencePlant?.watering;
  }

  /**
   * Obtém o número de horas de cooldown com base no tipo de planta e frequência de rega.
   */
  get cooldownHours(): number {
    const plantType = this.plantationPlant?.referencePlant?.plantType?.charAt(0).toUpperCase() +
      this.plantationPlant?.referencePlant?.plantType?.slice(1).toLowerCase() as PlantType;
    const wateringFrequency = this.plantationPlant?.referencePlant?.watering as WaterFrequency;

    if (!plantType || !wateringFrequency) {
      console.error('Missing plant type or watering frequency');
      return 0;
    }

    if (!(plantType in this.cooldownConfig)) {
      console.error(`Invalid plant type: ${plantType}`);
      return 0;
    }

    if (!(wateringFrequency in this.cooldownConfig[plantType])) {
      console.error(`Invalid watering frequency: ${wateringFrequency} for ${plantType}`);
      return 0;
    }

    return this.cooldownConfig[plantType][wateringFrequency];
  }

  /**
   * Atualiza a mensagem de cooldown da próxima colheita com base numa data.
   * @param nextHarvestDateStr Data da próxima colheita.
   */
  private updateHarvestCooldownMsgFromDate(nextHarvestDateStr: string) {
    const now = new Date();
    const nextHarvestDate = new Date(nextHarvestDateStr);
    const diffMs = nextHarvestDate.getTime() - now.getTime();

    if (diffMs <= 0) {
      this.harvestCooldownMsg = '✅ Ready to harvest!';
      return;
    }

    const diffMinutes = Math.floor(diffMs / 60000);
    const days = Math.floor(diffMinutes / (24 * 60));
    const hours = Math.floor((diffMinutes % (24 * 60)) / 60);
    const minutes = diffMinutes % 60;

    const msgParts = [];
    if (days > 0) msgParts.push(`${days}d`);
    if (hours > 0) msgParts.push(`${hours}h`);
    if (minutes > 0) msgParts.push(`${minutes}m`);

    this.harvestCooldownMsg = `⏳ Next harvest available in ${msgParts.join(' ')}`;
  }

  /**
   * Apenas para debug: imprime no console informações sobre a planta selecionada.
   */
  logPlantData() {
    console.log('Plant Type:', this.plantationPlant?.referencePlant?.plantType);
    console.log('Watering Frequency:', this.plantationPlant?.referencePlant?.watering);
    console.log('Calculated Cooldown:', this.cooldownHours);
  }


  /**
   * Verifica se a planta pode ser regada com base no cooldown desde a última rega.
   */
  canWater(): boolean {
    if (!this.plantationPlant?.lastWatered) return true;
    const lastWatered = new Date(this.plantationPlant.lastWatered);
    const elapsedHours = (Date.now() - lastWatered.getTime()) / 3600000;
    return elapsedHours >= this.cooldownHours;
  }

  /**
   * Atualiza o estado visual do cooldown, mostrando tempo restante.
   */
  private updateCooldownStatus() {
    if (!this.plantationPlant?.lastWatered) {
      this.remainingCooldown = '';
      return;
    }

    const lastWatered = new Date(this.plantationPlant.lastWatered);
    const elapsedHours = (Date.now() - lastWatered.getTime()) / 3600000;
    const remaining = this.cooldownHours - elapsedHours;

    if (remaining <= 0) {
      this.remainingCooldown = '';
      return;
    }

    let totalHours = Math.floor(remaining);
    let minutes = Math.round((remaining % 1) * 60);

    if (minutes === 60) {
      totalHours += 1;
      minutes = 0;
    }

    const days = Math.floor(totalHours / 24);
    const hours = totalHours % 24;

    this.remainingCooldown = `${days}d ${hours}h ${minutes}m remaining`;
  }


  /**
    * Rega a planta, atualizando a data da última rega e cooldown.
    * Também adiciona experiência à plantação associada.
    */
  waterPlants() {
    if (!this.plantationId) return;
    if (!this.plantInfoId) return;

    if (!this.canWater()) {
      alert('Plant is still in cooldown!');
      return;
    }

    this.plantationsService.waterPlant(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (updatedPlant) => {
          if (this.plantationPlant) {
            this.plantationPlant.lastWatered = updatedPlant.lastWatered;
            this.updateCooldownStatus();
            this.increaseExperience(false);
          }

        },
        error: (error) => {
          console.error("Watering Failed", error);
        }
      })
  }

  /**
   * Realiza a colheita da planta se estiver pronta, atualiza os dados e XP.
   */
  harvestPlant() {
    if (!this.canHarvest) {
      alert('Plant is not ready for harvest yet!');
      return;
    }

    this.plantationsService.harvestPlant(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (res) => {
          this.loadPlantationPlantDetails();
          this.increaseExperience(true);
        },
        error: (err) => {
          console.error('Harvest failed:', err);
          alert('Failed to harvest plant.');
        }
      });
  }

  /**
   * Verifica com o backend se a planta já pode ser colhida com base na data de plantação e recorrência.
   */
  checkHarvestStatus() {
    this.plantationsService.checkHarvest(this.plantationId, this.plantInfoId)
      .subscribe({
        next: (res) => {
          this.canHarvest = res.canHarvest;
          this.nextHarvestDate = res.nextHarvestDate;

          if (!res.canHarvest && res.nextHarvestDate) {
            this.updateHarvestCooldownMsgFromDate(res.nextHarvestDate);
          } else {
            this.harvestCooldownMsg = '✅ Ready to harvest!';
          }
        },
        error: (err) => {
          console.error('Error checking harvest status:', err);
          this.harvestCooldownMsg = '';
        }
      });
    }

  /**
   * Envia uma requisição ao backend para adicionar experiência à plantação.
   * @param isHarvesting Indica se a ação foi colheita (true) ou rega (false).
   */
  increaseExperience(isHarvesting: boolean) {
    if (!this.plantation) {
      console.error(`No plantation${this.plantation}`);
    }
    if (!this.plantationPlant) {
      console.error(`No plantationPlant${this.plantationPlant}`);
    }

    this.plantationsService.gainExperience(this.plantationId, this.plantInfoId, isHarvesting)
      .subscribe({
        next: () => {
        },
        error: (error) => {
          console.error("Experience Increase Failed", error);
        }
      })
  }

  /**
   * Retorna à página anterior no histórico do navegador.
   */
  goBack() {
    this.location.back();
  }
}
