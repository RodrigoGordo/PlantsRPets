import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface PlantPeriodWarningData {
  message: string;
  idealMonths: string[];
}

@Component({
  selector: 'app-plant-period-warning',
  standalone: false,
  
  templateUrl: './plant-period-warning.component.html',
  styleUrl: './plant-period-warning.component.css'
})

/**
 * Componente de aviso modal utilizado quando um utilizador tenta plantar uma planta fora do seu período ideal.
 * Apresenta as informações relevantes e permite ao utilizador confirmar ou cancelar a ação.
 */
export class PlantPeriodWarningComponent {

  /**
   * Construtor que injeta as dependências necessárias:
   * - `dialogRef`: Referência da instância da caixa de diálogo atual, usada para fechar o modal e devolver o resultado.
   * - `data`: Dados passados para o modal (ex.: nome da planta, mês atual, meses ideais).
   * 
   * @param dialogRef Referência do modal para controlo de fecho
   * @param data Dados passados para o modal com informações sobre o período de plantação
   */
  constructor(
    private dialogRef: MatDialogRef<PlantPeriodWarningComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PlantPeriodWarningData
  ) { }

  /**
   * Fecha o modal e confirma a ação de plantação fora do período ideal.
   */
  confirm(): void {
    this.dialogRef.close(true);
  }

  /**
   * Fecha o modal e cancela a ação de plantação.
   */
  cancel(): void {
    this.dialogRef.close(false);
  }
}
