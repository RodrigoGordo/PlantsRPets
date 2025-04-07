import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tip } from '../app/models/tip-model';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço que permite obter dicas de sustentabilidade associadas a uma planta.
 * Comunica com o controlador SustainabilityTipsController no backend.
 */
export class TipService {
  private apiUrl = 'api/SustainabilityTips';

  constructor(private http: HttpClient) { }

  /**
   * Obtém todas as dicas de sustentabilidade para uma determinada planta.
   * @param plantInfoId ID da planta
   * @returns Observable com a lista de dicas (Tip[])
   */
  getTipsByPlantId(plantInfoId: number): Observable<Tip[]> {
    return this.http.get<Tip[]>(`${this.apiUrl}/by-plant/${plantInfoId}`);
  }
}
