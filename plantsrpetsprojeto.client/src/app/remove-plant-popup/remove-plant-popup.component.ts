import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PlantationsService } from '../plantations.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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
  confirming: boolean = false;
  isSecondConfirmStep = false;
  removeForm: FormGroup;

  /**
   * Construtor do componente.
   * 
   * @param plantationsService Serviço responsável por interagir com a API de plantações.
   * @param dialogRef Referência ao diálogo atual (para o poder fechar após ações).
   * @param router Serviço de navegação do Angular.
   * @param data Dados recebidos do componente pai, incluindo nome da planta, quantidade máxima e IDs.
   */
  constructor(
    private fb: FormBuilder,
    private plantationsService: PlantationsService,
    private dialogRef: MatDialogRef<RemovePlantPopupComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: {
      plantName: string;
      maxQuantity: number;
      plantationId: number;
      plantInfoId: number;
    }
  ) {
    this.removeForm = this.fb.group({
      quantity: [{ value: 1, disabled: this.confirming || this.isSecondConfirmStep }, [
        Validators.required,
        Validators.min(1),
        Validators.max(data.maxQuantity)
      ]]
    });
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
   * Valida o valor introduzido ao perder o foco do input. Corrige automaticamente se necessário.
   */
  onInputBlur(): void {
    const control = this.removeForm.get('quantity');
    let value = parseInt(control?.value, 10);

    if (isNaN(value) || value < 1) {
      control?.setValue(1);
    } else if (value > this.data.maxQuantity) {
      control?.setValue(this.data.maxQuantity);
    }
  }

  /**
   * Confirma a remoção da planta. Se for uma remoção total, exige confirmação extra.
   * Após remoção com sucesso, fecha o diálogo e redireciona, se necessário.
   */
  confirmRemoval(): void {
    if (this.removeForm.invalid) return;

    const quantityToRemove = this.removeForm.value.quantity;

    if (quantityToRemove === this.data.maxQuantity && !this.isSecondConfirmStep) {
      this.isSecondConfirmStep = true;
      this.toggleQuantityControl();
      return;
    }

    this.confirming = true;
    this.plantationsService
      .removePlantFromPlantation(this.data.plantationId, this.data.plantInfoId, quantityToRemove)
      .subscribe({
        next: () => {
          this.confirming = false;

          if (quantityToRemove === this.data.maxQuantity) {
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

  toggleQuantityControl(): void {
    if (this.isSecondConfirmStep || this.confirming) {
      this.removeForm.get('quantity')?.disable();
    } else {
      this.removeForm.get('quantity')?.enable();
    }
  }

  /**
   * Cancela o segundo passo de confirmação (caso o utilizador mude de ideias).
   */
  cancelConfirmation(): void {
    this.isSecondConfirmStep = true;
    this.toggleQuantityControl();
  }

  /**
   * Fecha o diálogo sem realizar qualquer remoção.
   */
  cancel(): void {
    this.dialogRef.close(false);
  }
}
