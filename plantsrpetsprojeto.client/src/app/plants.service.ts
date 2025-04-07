import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlantInfo } from './models/plant-info';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável por interagir com a API relacionada com plantas.
 * Inclui funcionalidades para:
 * - Obter lista de plantas
 * - Consultar detalhes de uma planta
 * - Verificar se a época atual é adequada para plantar
 */
export class PlantsService {
  private apiUrl = 'api/plants';

  constructor(private http: HttpClient) { }

  /**
   * Obtém todas as plantas disponíveis na base de dados.
   * @returns Observable com um array de objetos PlantInfo
   */
  getPlants(): Observable<PlantInfo[]> {
    return this.http.get<PlantInfo[]>(this.apiUrl);
  }

  /**
   * Obtém os detalhes de uma planta específica, através do seu ID.
   * @param id ID da planta
   * @returns Observable com os dados da planta
   */
  getPlantById(id: number): Observable<PlantInfo> {
    return this.http.get<PlantInfo>(`${this.apiUrl}/${id}`);
  }

  /**
   * Verifica se o mês atual é apropriado para plantar uma determinada planta.
   * A decisão baseia-se na estação de colheita e no tempo médio de crescimento.
   * @param id ID da planta
   * @returns Observable com informação sobre a época de plantação ideal
   */
  getPlantingPeriodCheck(id: number): Observable<any> {
    return this.http.get<any>(`api/plants/check-planting-period/${id}`);
  }
}
