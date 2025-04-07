import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlantationsService } from '../plantations.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-remove-plant-popup',
  standalone: false,
  
  templateUrl: './remove-plant-popup.component.html',
  styleUrl: './remove-plant-popup.component.css'
})

/**
 * Componente responsável por gerir a lógica do popup de remoção de uma planta de uma plantação.
 * Permite ao utilizador definir a quantidade a remover, valida o input e confirma a remoção com um segundo passo caso a quantidade seja total.
 */
export class RemovePlantPopupComponent {
  _quantityToRemove: number = 1;
  inputValue: string = '1';
  confirming: boolean = false;
  isSecondConfirmStep = false;

  /**
   * Construtor do componente.
   * 
   * @param plantationsService Serviço responsável por interagir com a API de plantações.
   * @param dialogRef Referência ao diálogo atual (para o poder fechar após ações).
   * @param router Serviço de navegação do Angular.
   * @param data Dados recebidos do componente pai, incluindo nome da planta, quantidade máxima e IDs.
   */
  constructor(
    private plantationsService: PlantationsService,
    private dialogRef: MatDialogRef<RemovePlantPopupComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: {
      plantName: string;
      maxQuantity: number;
      plantationId: number;
      plantInfoId: number;
    }
  ) { }

  /**
   * Getter da quantidade a remover com base no input do utilizador, assegurando que é válido e dentro dos limites permitidos.
   */
  get quantityToRemove(): number {
    const value = parseInt(this.inputValue, 10);
    return isNaN(value) ? 0 : Math.min(value, this.data.maxQuantity);
  }

  /**
   * Setter da quantidade a remover, garantindo que o valor está dentro dos limites válidos.
   */
  set quantityToRemove(value: number) {
    if (!value || value < 1) {
      this._quantityToRemove = 1;
    } else if (value > this.data.maxQuantity) {
      this._quantityToRemove = this.data.maxQuantity;
    } else {
      this._quantityToRemove = value;
    }
  }

  /**
   * Atualiza o valor do input com o valor introduzido pelo utilizador.
   */
  onInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.inputValue = input.value;
  }

  /**
   * Valida o valor introduzido ao perder o foco do input. Corrige automaticamente se necessário.
   */
  onInputBlur(): void {
    const value = parseInt(this.inputValue, 10);

    if (isNaN(value) || value < 1) {
      this.inputValue = '1';
    } else if (value > this.data.maxQuantity) {
      this.inputValue = this.data.maxQuantity.toString();
    }
  }

  /**
   * Confirma a remoção da planta. Se for uma remoção total, exige confirmação extra.
   * Após remoção com sucesso, fecha o diálogo e redireciona, se necessário.
   */
  confirmRemoval(): void {
    if (this.quantityToRemove === this.data.maxQuantity && !this.isSecondConfirmStep) {
      this.isSecondConfirmStep = true;
      return;
    }

    this.confirming = true;
    this.plantationsService
      .removePlantFromPlantation(this.data.plantationId, this.data.plantInfoId, this.quantityToRemove)
      .subscribe({
        next: () => {
          this.confirming = false;

          if (this.quantityToRemove === this.data.maxQuantity) {
            this.dialogRef.close(true);
            this.router.navigate(['/plantation', this.data.plantationId]);
          } else {
            this.dialogRef.close(true);
          }
        },
        error: (err) => {
          console.error('Error removing plant:', err);
          this.confirming = false;
        }
      });
  }

  /**
   * Cancela o segundo passo de confirmação (caso o utilizador mude de ideias).
   */
  cancelConfirmation(): void {
    this.isSecondConfirmStep = true;
  }

  /**
   * Fecha o diálogo sem realizar qualquer remoção.
   */
  cancel(): void {
    this.dialogRef.close(false);
  }
}
