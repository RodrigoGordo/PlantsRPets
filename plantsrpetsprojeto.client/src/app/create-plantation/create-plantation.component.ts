import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantTypesService } from '../plant-type.service';
import { PlantType } from '../models/plant-type.model';
import { Location } from '../models/location.model';

@Component({
  selector: 'app-create-plantation',
  standalone: false,
  
  templateUrl: './create-plantation.component.html',
  styleUrl: './create-plantation.component.css'
})

/**
 * Componente responsável por exibir o formulário de criação de uma nova plantação.
 * Permite ao utilizador selecionar o tipo de planta e atribuir um nome à plantação.
 */
export class CreatePlantationComponent {
  plantationForm: FormGroup;
  plantTypes: PlantType[] = [];
  newLocation!: Location;

  /**
   * Construtor do componente que injeta serviços necessários:
   * 
   * @param fb FormBuilder para configurar o formulário reativo
   * @param dialogRef Referência ao diálogo modal
   * @param plantationsService Serviço que envia dados da plantação ao backend
   * @param plantTypesService Serviço que obtém os tipos de plantas disponíveis
   */
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<CreatePlantationComponent>,
    private plantationsService: PlantationsService,
    private plantTypesService: PlantTypesService
  ) {
    this.plantationForm = this.fb.group({
      plantationName: ['', [Validators.required, Validators.minLength(3)]], 
      plantTypeId: ['', Validators.required]
    });
  }

  /**
   * Método do ciclo de vida do componente. É executado após a renderização inicial.
   * Responsável por carregar os tipos de planta do backend.
   */
  ngOnInit(): void {
    this.loadPlantTypes();
  }

  /**
   * Recolhe os tipos de plantas disponíveis através da API.
   * Filtra resultados duplicados com base no `plantTypeId`.
   */
  loadPlantTypes(): void {
    this.plantTypesService.getPlantTypes().subscribe({
      next: (data) => {
        console.log("Dados recebidos do backend:", data);
        this.plantTypes = data.filter(
          (type, index, self) =>
            index === self.findIndex((t) => t.plantTypeId === type.plantTypeId)
        );
      },
      error: (error) => console.error("Erro ao carregar os tipos de plantas:", error)
    });
  }

  /**
   * Submete o formulário para criar uma nova plantação.
   * Envia os dados para a API e fecha o diálogo após sucesso.
   */
  createPlantation(): void {
    if (this.plantationForm.valid) {
      const requestData = {
        plantationName: this.plantationForm.value.plantationName,
        plantTypeId: this.plantationForm.value.plantTypeId,
        location: this.newLocation
      };

      this.plantationsService.createPlantation(requestData).subscribe({
        next: () => { this.dialogRef.close(true); },
        error: (error) => { console.error("Erro ao criar plantação:", error); }
      });
    }
  }

  /**
 * Atualiza a localização selecionada pelo utilizador para a nova plantação.
 * 
 * @param location - Objeto de localização selecionado (cidade, região, país, coordenadas).
 */
  onLocationSelected(location: Location): void {
    this.newLocation = location;
  }

   /**
   * Fecha a janela modal sem realizar nenhuma ação.
   */
  closeDialog(): void {
    this.dialogRef.close();
  }
}
