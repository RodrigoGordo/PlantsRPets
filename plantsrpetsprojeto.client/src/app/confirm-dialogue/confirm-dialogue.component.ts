import { Component } from '@angular/core';

/**
 * Componente responsável por exibir uma caixa de diálogo de confirmação.
 * Este componente é utilizado para confirmar ações críticas, como exclusão de dados,
 * alterações importantes ou qualquer ação que requeira uma validação adicional do utilizador.
 */
@Component({
  selector: 'app-confirm-dialogue',
  standalone: false,
  
  templateUrl: './confirm-dialogue.component.html',
  styleUrl: './confirm-dialogue.component.css'
})
export class ConfirmDialogueComponent {

}
