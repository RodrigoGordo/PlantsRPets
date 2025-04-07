import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável por obter a lista de tipos de planta disponíveis.
 * Usa a API /api/plant-types no backend.
 */
export class PlantTypesService {
  private apiUrl = 'api/plant-types';

  constructor(private http: HttpClient) { }

  /**
   * Devolve a lista de tipos de planta disponíveis no sistema.
   * @returns Observable com array de objetos contendo nome e ID do tipo de planta
   */
  getPlantTypes(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
