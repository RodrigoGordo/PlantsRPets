import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável por obter métricas relacionadas com a atividade do utilizador na aplicação.
 * Permite consultar:
 * - Contagens de eventos por tipo
 * - Atividade ao longo do tempo
 * - Distribuição de tipos de plantas nas plantações
 */
export class MetricsService {
  private baseUrl = 'api/metrics';

  constructor(private http: HttpClient) { }

  /**
   * Obtém o número de eventos de cada tipo (rega, plantação, colheita) num determinado intervalo temporal.
   * @param timeFrame Intervalo de tempo ("day", "week", "month", "year")
   * @returns Observable com as contagens agrupadas por tipo de evento
   */
  getActivityCounts(timeFrame: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/activity-counts?timeFrame=${timeFrame}`);
  }

  /**
   * Obtém os eventos de um tipo específico ao longo do tempo (por data).
   * Útil para gráficos de evolução da atividade.
   * @param eventType Tipo de evento (ex: "Watering", "Harvest")
   * @param timeFrame Intervalo de tempo desejado
   * @returns Observable com os dados agrupados por data
   */
  getActivityByDate(eventType: string, timeFrame: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/activity-by-date?eventType=${eventType}&timeFrame=${timeFrame}`);
  }

  /**
   * Obtém a distribuição de tipos de planta (e respetiva quantidade) presentes nas plantações do utilizador.
   * @returns Observable com a distribuição dos tipos de planta
   */
  getPlantTypeDistribution(): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/plant-type-distribution`);
  }
}
