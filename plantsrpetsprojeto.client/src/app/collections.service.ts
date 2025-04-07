import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável por interagir com a coleção de pets do utilizador.
 * Permite operações como:
 * - Obter pets não colecionados
 * - Atualizar estado de posse
 * - Obter pets favoritos
 */
export class CollectionService {
  private apiUrl = 'api/collections';

  constructor(private http: HttpClient) { }

  /**
   * Obtém 3 pets aleatórios que o utilizador ainda não possui.
   * @returns Observable com uma lista de até 3 pets
   */
  getRandomUnownedPets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-unowned`);
  }

  /**
   * Atualiza o estado de posse (IsOwned) de um pet específico.
   * @param petId ID do pet a atualizar
   * @param isOwned Novo valor do estado de posse (true ou false)
   * @returns Observable com a resposta da API
   */
  updateOwnedStatus(petId: number, isOwned: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/owned/${petId}`, { isOwned });
  }

  /**
   * Obtém todos os pets marcados como favoritos na coleção do utilizador.
   * @returns Observable com a lista de pets favoritos
   */
  getFavoritePetsInCollection(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/favoritePets`);
  }
}
