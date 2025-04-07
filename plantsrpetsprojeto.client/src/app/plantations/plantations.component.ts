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

/**
 * Componente responsável por exibir a lista de plantações do utilizador.
 * Também permite criar novas plantações e eliminar plantações existentes.
 */
export class PlantationsComponent implements OnInit {
  plantations: any[] = [];

  /**
   * Injeta os serviços necessários:
   * - `PlantationsService`: responsável por interações com a API relacionadas a plantações.
   * - `MatDialog`: serviço do Angular Material para abrir caixas de diálogo modais.
   * 
   * @param plantationsService Serviço de API para gerir plantações.
   * @param dialog Serviço para abrir diálogos modais.
   */
  constructor(private plantationsService: PlantationsService, private dialog: MatDialog) { }

  /**
   * Método chamado no ciclo de vida `OnInit` para carregar automaticamente
   * as plantações do utilizador ao entrar no componente.
   */
  ngOnInit(): void {
    this.loadPlantations();
    console.log(this.plantations);
  }

  /**
   * Carrega todas as plantações do utilizador a partir do backend.
   */
  loadPlantations(): void {
    this.plantationsService.getUserPlantations().subscribe({
      next: (data) => {
        this.plantations = data;
        console.log("Plantações");
        console.log(this.plantations);
      },
      error: (error) => {
        console.error('Error fetching plantations:', error);
      }
    });
  }

  /**
   * Abre o diálogo modal para criação de nova plantação.
   * Após o diálogo ser fechado com sucesso, recarrega a lista de plantações.
   */
  openCreateDialog(): void {
    const dialogRef = this.dialog.open(CreatePlantationComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadPlantations();
    });
  }

  /**
   * Remove uma plantação com base no seu ID.
   * Após remoção bem-sucedida, recarrega a lista de plantações.
   * 
   * @param id O ID da plantação a remover.
   */
  deletePlantation(id: number): void {
    this.plantationsService.deletePlantation(id).subscribe(() => this.loadPlantations());
  }
}
