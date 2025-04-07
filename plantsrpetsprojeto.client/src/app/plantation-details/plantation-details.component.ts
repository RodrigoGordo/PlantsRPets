import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AddPlantComponent } from '../add-plant/add-plant.component';
import { PlantationsService } from '../plantations.service';
import { Plantation } from '../models/plantation.model';
import { PlantInfo } from '../models/plant-info';
import { CollectionService } from '../collections.service';
import { PetRewardPopupComponent } from '../pet-reward-popup/pet-reward-popup.component';

@Component({
  selector: 'app-plantation-details',
  standalone: false,
  
  templateUrl: './plantation-details.component.html',
  styleUrl: './plantation-details.component.css'
})

/**
 * Componente responsável por apresentar os detalhes de uma plantação específica,
 * incluindo as plantas nela contidas e ações como adicionar plantas ou usar level-ups acumulados.
 */
export class PlantationDetailsComponent implements OnInit {
  plantation: Plantation = {
    plantationId: 0,
    plantationName: '',
    plantTypeId: 0,
    plantTypeName: '',
    lastWatered: new Date(),
    plantingDate: new Date(),
    harvestDate: new Date(),
    growthStatus: '',
    experiencePoints: 0,
    level: 0,
    bankedLevelUps: 0,
    location: undefined,
    plantationPlants: []
  };

  plantationPlants: PlantInfo[] = [];
  loading: boolean = true;
  errorMessage: string = '';

  isCollectionFull: boolean = false;

  /**
   * Construtor do componente.
   * 
   * @param route - Serviço para acesso a parâmetros da rota.
   * @param plantationsService - Serviço para operações relacionadas à plantação.
   * @param collectionService - Serviço para acesso à coleção de pets do utilizador.
   * @param dialog - Serviço de diálogo (mat-dialog) para popups.
   */
  constructor(
    private route: ActivatedRoute,
    private plantationsService: PlantationsService,
    private collectionService: CollectionService,
    private dialog: MatDialog
  ) { }

  /**
   * Ciclo de vida que ocorre quando o componente é inicializado.
   * Inicia o carregamento dos dados da plantação e das suas plantas.
   */
  ngOnInit(): void {
    this.loadPlantation();
    this.loadPlantationPlants();
  }

  /**
   * Carrega os detalhes da plantação com base no ID da rota.
   */
  loadPlantation(): void {
    const plantationId = Number(this.route.snapshot.paramMap.get('id'));
    if (!plantationId) {
      this.errorMessage = "Invalid Plantation ID";
      this.loading = false;
      return;
    }

    this.plantationsService.getPlantationById(plantationId).subscribe({
      next: (data) => {
        this.plantation = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = "Failed to load plantation details.";
        this.loading = false;
      }
    });

  }

  /**
   * Carrega a lista de plantas associadas à plantação atual.
   */
  loadPlantationPlants(): void {
    const plantationId = Number(this.route.snapshot.paramMap.get('id'));
    if (!plantationId) return;

    this.plantationsService.getPlantsInPlantation(plantationId).subscribe({
      next: (data) => {
        console.log("Plants received:", data);
        this.plantationPlants = data.map(pp => pp.referencePlant);
      },
      error: () => {
        this.errorMessage = "Failed to load plants.";
      }
    });
  }

  /**
   * Abre um diálogo para adicionar uma nova planta à plantação.
   */
  openAddPlantDialog(): void {
    const dialogRef = this.dialog.open(AddPlantComponent, {
      width: '400px',
      data: this.plantation.plantationId
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPlantationPlants();
      }
    });
  }

  /**
   * Utiliza um "banked level-up" da plantação, atualizando o estado local após sucesso.
   */
  useBankedLevelUp(): void {
    if (!this.plantation) {
      console.error('Plantation data not loaded');
      return;
    }

    console.log(this.plantation.bankedLevelUps);
    this.plantationsService.usePlantationBankedLevelUp(this.plantation.plantationId)
      .subscribe({
        next: (updatedPlantation) => {
          if (this.plantation) {
            this.plantation.bankedLevelUps = updatedPlantation.bankedLevelUps;
            console.log("After using banked Level Up");
            console.log(this.plantation.bankedLevelUps);
            console.log(updatedPlantation);
          }
        },
        error: (error) => {
          console.error("Error when trying to decrement banked level ups", error);
        }
      })
  }

  /**
   * Abre um modal com pets de recompensa aleatórios.
   * Caso o utilizador escolha um, a recompensa é registada e o level-up consumido.
   */
  openRewardPopup(): void {
    this.collectionService.getRandomUnownedPets().subscribe({
      next: (pets) => {
        if (pets.length > 0) {
          const dialogRef = this.dialog.open(PetRewardPopupComponent, {
            width: '600px',
            data: { pets }
          });

          dialogRef.afterClosed().subscribe(result => {
            if (result) {
              console.log('Reward claimed!');
              this.useBankedLevelUp();
            }
          });
        } else {
          this.isCollectionFull = true;
        }
      },
      error: (err) => {
        console.error('Failed to load reward pets:', err);
      }
    });
  }

}
