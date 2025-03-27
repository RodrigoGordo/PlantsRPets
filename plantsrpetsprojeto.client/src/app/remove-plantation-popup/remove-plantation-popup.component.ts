import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-remove-plantation-popup',
  standalone: false,
  
  templateUrl: './remove-plantation-popup.component.html',
  styleUrl: './remove-plantation-popup.component.css'
})
export class RemovePlantationPopupComponent {
  /**
   * Construtor do componente que gere a referência da caixa de diálogo.
   * 
   * @param dialogRef - Referência da instância do diálogo atual, utilizada para fechar o modal e retornar o resultado.
   */
  constructor(private dialogRef: MatDialogRef<RemovePlantationPopupComponent>) { }


  /**
   * Confirma a ação de remoção da plantação e fecha o diálogo, retornando o estado 'confirm'.
   */
  confirm(): void {
    this.dialogRef.close('confirm');
  }

  /**
   * Cancela a ação de remoção da plantação e fecha o diálogo, retornando o estado 'cancel'.
   */
  cancel(): void {
    this.dialogRef.close('cancel');
  }
}
