import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { PlantationsService } from '../plantations.service';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

@Component({
  selector: 'app-add-plant',
  standalone: false,
  
  templateUrl: './add-plant.component.html',
  styleUrl: './add-plant.component.css'
})

/**
 * Componente responsável por permitir ao utilizador adicionar uma nova planta a uma plantação existente.
 * Fornece formulário com filtro e validação, consulta as plantas disponíveis e efetua a submissão da ação.
 */
export class AddPlantComponent implements OnInit {
  addPlantForm: FormGroup;
  plants: PlantInfo[] = [];
  filteredPlants: PlantInfo[] = [];
  plantFilter = new FormControl('');

  /**
   * Construtor do componente que injeta os serviços e inicializa o formulário.
   * 
   * @param fb - FormBuilder para criar o formulário reativo
   * @param dialogRef - Referência ao diálogo atual, usada para fechar o modal
   * @param plantationsService - Serviço para operações relacionadas com plantações
   * @param plantsService - Serviço para obter as plantas disponíveis
   * @param plantationId - ID da plantação (recebido via MAT_DIALOG_DATA)
   */
  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddPlantComponent>,
    private plantationsService: PlantationsService,
    private plantsService: PlantsService,
    @Inject(MAT_DIALOG_DATA) public plantationId: number
  ) {
    this.addPlantForm = this.fb.group({
      plantInfoId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
  }

  /**
   * Método executado na inicialização do componente.
   * Carrega a lista de plantas e configura o filtro reativo com base no input.
   */
  ngOnInit(): void {
    this.loadPlants();

    this.plantFilter.valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? this.filterPlants(value) : this.plants))
    ).subscribe(filtered => this.filteredPlants = filtered);
  }

  /**
   * Obtém a lista de plantas do serviço e ordena por nome.
   */
  loadPlants(): void {
    this.plantsService.getPlants().subscribe({
      next: (data) => {
        this.plants = data.sort((a, b) => a.plantName.localeCompare(b.plantName));
        this.filteredPlants = this.plants;
      },
      error: (error) => {
        console.error('Error loading plants:', error);
      }
    });
  }

  /**
   * Filtra as plantas com base no valor de pesquisa.
   * @param value - Valor a ser usado como filtro
   * @returns Lista de plantas cujo nome inclui o termo pesquisado
   */
  filterPlants(value: string): PlantInfo[] {
    const filterValue = value.toLowerCase();
    return this.plants.filter(plant => plant.plantName.toLowerCase().includes(filterValue));
  }

  /**
   * Exibe o nome da planta no campo do autocompletar.
   * @param plant - Objeto PlantInfo
   * @returns Nome da planta ou string vazia
   */
  displayPlantName(plant?: PlantInfo): string {
    return plant ? plant.plantName : '';
  }

  /**
   * Atualiza o campo de ID da planta ao selecionar um item do autocompletar.
   * @param event - Evento de seleção do MatAutocomplete
   */
  selectPlant(event: MatAutocompleteSelectedEvent) {
    const selectedPlant = event.option.value as PlantInfo;
    this.addPlantForm.patchValue({
      plantInfoId: selectedPlant.plantInfoId,
    });

    this.plantFilter.setValue(selectedPlant.plantName, { emitEvent: false });
  }

  /**
   * Submete o formulário para adicionar a planta à plantação.
   * Fecha o diálogo em caso de sucesso.
   */
  addPlant(): void {
    if (this.addPlantForm.valid) {
      const requestData = {
        plantInfoId: this.addPlantForm.value.plantInfoId,
        quantity: this.addPlantForm.value.quantity
      };

      this.plantationsService.addPlantToPlantation(this.plantationId, requestData).subscribe({
        next: () => {
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Error adding plant:', error);
        }
      });
    }
  }

  /**
   * Fecha o diálogo sem adicionar nenhuma planta.
   */
  closeDialog(): void {
    this.dialogRef.close();
  }
}
