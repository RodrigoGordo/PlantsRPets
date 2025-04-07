import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

/**
* Serviço responsável por obter informações meteorológicas da API.
* Permite obter previsões por nome de cidade ou por coordenadas geográficas.
*/
export class WeatherService {
  constructor(private http: HttpClient) { }

  /**
   * Obtém a previsão do tempo com base no nome da cidade.
   * @param city Nome da cidade (ex: 'Lisboa')
   * @returns Observable com os dados meteorológicos
   */
  getWeatherByCity(city: string): Observable<any> {
    return this.http.get(`/api/weather/city/${city}`);
  }

  /**
   * Obtém a previsão do tempo com base nas coordenadas geográficas.
   * @param lat Latitude
   * @param lon Longitude
   * @returns Observable com os dados meteorológicos
   */
  getWeatherByCoords(lat: number, lon: number): Observable<any> {
    return this.http.get(`/api/weather/coords/${lat}/${lon}`);
  }
}
