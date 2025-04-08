import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PlantationsService } from '../plantations.service';
import { RecentActivityService } from '../recent-activity.service';
import { MatDialog } from '@angular/material/dialog';
import { RemovePlantationPopupComponent } from '../remove-plantation-popup/remove-plantation-popup.component';
import { PlantationPlant } from '../models/plantation-plant';
import { Plantation } from '../models/plantation.model';
import { CityService } from '../city.service';
import { Location } from '../models/location.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-plantation-card',
  standalone: false,
  
  templateUrl: './plantation-card.component.html',
  styleUrl: './plantation-card.component.css'
})

/**
 * Componente responsável por apresentar o cartão de uma plantação individual.
 * Permite editar o nome, apagar a plantação e registar a sua seleção como atividade recente.
 */
export class PlantationCardComponent {
  @Input() plantation!: any;
  @Output() updated = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<void>();

  isEditing: boolean = false;
  newPlantationName: string = '';
  newLocation!: Location;

  citySearchTerm: string = '';

  /**
   * Construtor do componente.
   * 
   * @param plantationsService - Serviço para operações CRUD sobre plantações.
   * @param recentActivity - Serviço responsável por registar plantações recentemente acedidas.
   * @param CityService - Serviço responsável por identificar a localização das plantações
   * @param dialog - Serviço para abrir caixas de diálogo (mat-dialog).
   */
  constructor(private plantationsService: PlantationsService, private recentActivity: RecentActivityService, private cityService: CityService, private dialog: MatDialog) { }

  /**
   * Ativa o modo de edição e carrega o nome atual da plantação.
   */
  enableEdit(): void {
    this.isEditing = true;
    this.newPlantationName = this.plantation.plantationName;
    if (this.plantation.location) {
      this.newLocation = this.plantation.location;
      this.citySearchTerm = `${this.plantation.location.city}, ${this.plantation.location.region}, ${this.plantation.location.country}`;
    }
  }

  /**
   * Guarda o novo nome da plantação e emite evento de atualização.
   */
  saveEdit(): void {
    if (this.plantation.plantationName == this.newPlantationName) {
      console.log('Plantation name is the same, no need to update.');
    } else {
      this.plantationsService.updatePlantationName(this.plantation.plantationId, this.newPlantationName)
        .subscribe(() => {
          this.plantation.plantationName = this.newPlantationName;
          this.isEditing = false;
          this.updated.emit();
        });
    }

    if (this.newLocation == this.plantation.location) {
      console.log('Location name is the same, no need to update.');

    } else {
      this.plantationsService.updateLocation(this.plantation.plantationId, this.newLocation)
        .subscribe(() => {
          this.plantation.location = this.newLocation;
          this.isEditing = false;
          this.updated.emit();
        });
    }

    this.cancelEdit();
  }

  /**
   * Cancela a edição e volta ao estado original.
   */
  cancelEdit(): void {
    this.isEditing = false;
  }

  /**
   * Abre diálogo de confirmação para remover a plantação.
   * Se confirmado, apaga a plantação e remove da lista de atividade recente.
   */
  remove(): void {
    const dialogRef = this.dialog.open(RemovePlantationPopupComponent, {
      width: '360px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.plantationsService.deletePlantation(this.plantation.plantationId)
          .subscribe(() => this.deleted.emit());
        this.recentActivity.removePlantation(this.plantation.plantationId);
      }
    });
  }

  /**
   * Regista a plantação como recentemente acedida no serviço de atividade.
   */
  onPlantationSelected(): void {
    this.recentActivity.savePlantation(this.plantation.plantationId);
    console.log(this.recentActivity.getRecentPlantations());
  }

  getTotalPlants(): number {
    return this.plantation.plantationPlants.reduce((sum: number, plant: PlantationPlant) =>
      sum + (plant.quantity || 0), 0) || 0;
  }

  onLocationSelected(location: Location): void {
    this.newLocation = location;
  }
}


