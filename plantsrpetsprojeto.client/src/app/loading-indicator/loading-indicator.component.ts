import { Component, Input } from '@angular/core';


@Component({
  selector: 'app-loading-indicator',
  standalone: false,
  
  templateUrl: './loading-indicator.component.html',
  styleUrl: './loading-indicator.component.css'
})

  /**
   * Componente responsável por exibir um indicador de carregamento (loading spinner).
   * Este componente é utilizado para informar o utilizador de que uma operação está em curso,
   * como o carregamento de dados ou o processamento de uma ação.
   */
export class LoadingIndicatorComponent {

  /**
   * Propriedade que controla a visibilidade do indicador de carregamento.
   * Quando definida como `true`, o spinner é exibido; caso contrário, permanece oculto.
   */
  @Input() isLoading: boolean = false;
}
