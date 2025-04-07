import { Component, Input, Output, EventEmitter, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

interface PetItem {
  petId: number;
  name: string;
  type: string;
  details: string;
  battleStats: string;
  imageUrl: string;
}

@Component({
  selector: 'app-pet-reward-popup',
  standalone: false,
  
  templateUrl: './pet-reward-popup.component.html',
  styleUrl: './pet-reward-popup.component.css'
})

/**
 * Componente de popup que permite ao utilizador selecionar uma recompensa (pet) após subir de nível.
 * Apresenta uma lista de pets disponíveis e permite escolher um para adicionar à coleção.
 */
export class PetRewardPopupComponent {
  pets: PetItem[] = [];
  selectedPetId: number | null = null;
  confirming: boolean = false;

  /**
   * Construtor do componente.
   * 
   * @param http - Serviço HTTP usado para atualizar o estado de posse do pet selecionado.
   * @param dialogRef - Referência ao diálogo atual para poder fechá-lo após a seleção.
   * @param data - Dados injetados no diálogo, incluindo os pets disponíveis para seleção.
   */
  constructor(
    private http: HttpClient,
    private dialogRef: MatDialogRef<PetRewardPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.pets = data.pets;
  }

  /**
   * Define o pet selecionado pelo utilizador.
   * 
   * @param petId - ID do pet escolhido.
   */
  selectPet(petId: number): void {
    this.selectedPetId = petId;
  }

  /**
   * Confirma a seleção do pet e marca-o como "owned" na coleção do utilizador.
   * Fecha o popup após atualização bem-sucedida.
   */
  confirmSelection(): void {
    if (this.selectedPetId === null) return;

    this.confirming = true;
    this.http.put(`/api/collections/owned/${this.selectedPetId}`, { isOwned: true }).subscribe({
      next: () => {
        this.confirming = false;
        this.dialogRef.close(true);
      },
      error: (err) => {
        console.error('Error claiming pet reward:', err);
        this.confirming = false;
      }
    });
  }

}
