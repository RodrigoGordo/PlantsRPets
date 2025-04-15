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
  formError: string | null = null;

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
      plantInfo: [null, Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]]
    });
  }

  /**
   * Método executado na inicialização do componente.
   * Carrega a lista de plantas e configura o filtro reativo com base no input.
   */
  ngOnInit(): void {
    this.loadPlants();

    this.addPlantForm.get('plantInfo')?.valueChanges.pipe(
      startWith(''),
      map(value => typeof value === 'string' ? this.filterPlants(value) : this.plants)
    ).subscribe(filtered => this.filteredPlants = filtered);

    this.addPlantForm.get('plantInfo')?.valueChanges.subscribe(() => {
      this.validateQuantityAgainstLimit();
    });

    this.addPlantForm.get('quantity')?.valueChanges.subscribe(() => {
      this.validateQuantityAgainstLimit();
    });
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
   * Getter que verifica se o valor atual do campo de quantidade excede o limite máximo permitido (1000).
   * Usado para desativar o botão de submissão e mostrar aviso ao utilizador.
   * @returns true se a quantidade for superior a 1000, false caso contrário.
   */
  get quantityExceedsLimit(): boolean {
    const quantity = this.addPlantForm.get('quantity')?.value;
    return quantity > 1000;
  }

  /**
   * Impede a introdução de caracteres inválidos no campo de quantidade.
   * Só permite números (0-9).
   */
  preventInvalidInput(event: KeyboardEvent): void {
    const allowedKeys = ['Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'Tab'];
    if (!/^\d$/.test(event.key) && !allowedKeys.includes(event.key)) {
      event.preventDefault();
    }
  }

  /**
   * Impede colar conteúdo inválido no campo de quantidade.
   * Se o valor colado não for só números, cancela o evento.
   */
  preventPaste(event: ClipboardEvent): void {
    const pastedInput: string = event.clipboardData?.getData('text') ?? '';
    if (!/^\d+$/.test(pastedInput)) {
      event.preventDefault();
    }
  }

  /**
  * Valida dinamicamente a quantidade inserida em relação à quantidade já existente.
  * Atualiza o formError com uma mensagem clara e desativa o botão se necessário.
  */
  validateQuantityAgainstLimit(): void {
    this.formError = null;

    const plant: PlantInfo = this.addPlantForm.get('plantInfo')?.value;
    const quantity: number = this.addPlantForm.get('quantity')?.value;

    if (!plant || !quantity || isNaN(quantity) || quantity < 1) {
      return;
    }

    const plantInfoId = plant.plantInfoId;

    this.plantationsService.getPlantationPlantById(this.plantationId, plantInfoId).subscribe({
      next: (plantData) => {
        const currentQuantity = plantData.quantity ?? 0;
        const total = currentQuantity + quantity;

        if (currentQuantity >= 1000) {
          this.formError = `You already have 1000 units of this plant. You cannot add more.`;
        } else if (total > 1000) {
          const remaining = 1000 - currentQuantity;
          this.formError = `Limit exceeded: You already have ${currentQuantity} units of this plant. You can only add up to ${remaining} more.`;
        }
      },
      error: (error) => {
        if (error.status !== 404) {
          console.error('Error validating quantity:', error);
          this.formError = 'There was a problem verifying the current quantity. Please try again later.';
        }
      }
    });
  }

  /**
  * Método executado quando o campo de quantidade perde o foco (blur).
  * Garante que o valor inserido seja um número entre 1 e 1000.
  * Se o valor for inválido (vazio, menor que 1 ou maior que 1000), será automaticamente corrigido para o limite mais próximo.
  */
  onQuantityBlur(): void {
    const control = this.addPlantForm.get('quantity');
    let value = parseInt(control?.value, 10);

    if (isNaN(value) || value < 1) {
      control?.setValue(1);
    } else if (value > 1000) {
      control?.setValue(1000);
    }
  }

  /**
   * Submete o formulário para adicionar a planta à plantação.
   * Fecha o diálogo em caso de sucesso.
   */
  addPlant(): void {
    this.formError = null;

    if (this.addPlantForm.valid && !this.formError) {
      const selectedPlant: PlantInfo = this.addPlantForm.value.plantInfo;
      const quantity: number = this.addPlantForm.value.quantity;

      const requestData = {
        plantInfoId: selectedPlant.plantInfoId,
        quantity
      };

      this.plantationsService.addPlantToPlantation(this.plantationId, requestData).subscribe({
        next: () => this.dialogRef.close(true),
        error: (error) => {
          console.error('Error adding plant:', error);
          this.formError = 'There was an error adding the plant. Please try again.';
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
