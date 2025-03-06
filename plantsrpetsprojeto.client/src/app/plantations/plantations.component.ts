import { Component, OnInit } from '@angular/core';
import { PlantationsService } from '../plantations.service';
import { MatDialog } from '@angular/material/dialog';
import { Plantation } from '../models/plantation.model';
import { CreatePlantationComponent } from '../create-plantation/create-plantation.component';


/**
 * Componente responsável pela gestão das plantações do utilizador.
 * Este componente servirá para exibir, adicionar, editar e remover plantações, bem como visualizar o progresso de cada uma.
 */
@Component({
  selector: 'app-plantations',
  standalone: false,
  
  templateUrl: './plantations.component.html',
  styleUrl: './plantations.component.css'
})
export class PlantationsComponent implements OnInit {
  plantations: any[] = [];

  constructor(private plantationsService: PlantationsService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.loadPlantations();
  }

  loadPlantations(): void {
    this.plantationsService.getUserPlantations().subscribe(
      (data: Plantation[]) => this.plantations = data,
      (error: any) => console.error('Error fetching plantations:', error)
    );
  }

  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreatePlantationComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadPlantations();
    });
  }

  deletePlantation(id: number): void {
    this.plantationsService.deletePlantation(id).subscribe(() => this.loadPlantations());
  }
}
