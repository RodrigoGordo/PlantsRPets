import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';


@Component({
  selector: 'app-logout-confirmation',
  standalone: false,
  
  templateUrl: './logout-confirmation.component.html',
  styleUrl: './logout-confirmation.component.css'
})

/**
 * Componente responsável por exibir uma caixa de diálogo de confirmação de logout.
 * Permite que o utilizador confirme ou cancele a ação de terminar sessão.
 */
export class LogoutConfirmationComponent {

  /**
   * Construtor do componente que gere a referência da caixa de diálogo.
   * 
   * @param dialogRef - Referência da instância do diálogo atual, utilizada para fechar o modal e retornar o resultado.
   */
  constructor(private dialogRef: MatDialogRef<LogoutConfirmationComponent>) { }


  /**
   * Confirma a ação de logout e fecha o diálogo, retornando o estado 'confirm'.
   */
  confirm(): void {
    this.dialogRef.close('confirm');
  }

  /**
   * Cancela a ação de logout e fecha o diálogo, retornando o estado 'cancel'.
   */
  cancel(): void {
    this.dialogRef.close('cancel');
  }
}
