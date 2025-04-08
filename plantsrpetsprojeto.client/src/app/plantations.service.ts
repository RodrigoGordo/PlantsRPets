import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlantInfo } from './models/plant-info';
import { PlantationPlant } from './models/plantation-plant';
import { Plantation } from './models/plantation.model';
import { Location } from './models/location.model';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável pela comunicação com a API relacionada com plantações.
 * Inclui funcionalidades para:
 * - Gestão de plantações do utilizador
 * - Manipulação de plantas dentro das plantações
 * - Rega, colheita, XP e evolução das plantações
 */
export class PlantationsService {
  private apiUrl = 'api/plantations';

  constructor(private http: HttpClient) { }

  /**
   * Obtém todas as plantações associadas ao utilizador autenticado.
   * @returns Observable com uma lista de plantações
   */
  getUserPlantations(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Obtém os detalhes de uma plantação específica.
   * @param id ID da plantação
   * @returns Observable com os dados da plantação
   */
  getPlantationById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }


  /**
   * Cria uma nova plantação com nome e tipo de planta especificados.
   * @param plantationData Dados da nova plantação (nome e tipo)
   * @returns Observable com a resposta da API
   */
  // Cria uma nova plantação
  createPlantation(plantationData: { plantationName: string; plantTypeId: number; location: Location }): Observable<any> {
    return this.http.post<any>(this.apiUrl, plantationData);
  }

  /**
   * Atualiza o nome de uma plantação específica.
   * @param id ID da plantação
   * @param newName Novo nome da plantação
   * @returns Observable com o resultado da operação
   */
  updatePlantationName(id: number, newName: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, { plantationName: newName });
  }

  /**
   * Obtém todas as plantas presentes numa plantação.
   * @param plantationId ID da plantação
   * @returns Observable com a lista de plantas da plantação
   */
  getPlantsInPlantation(plantationId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${plantationId}/plants`);
  }

  /**
   * Adiciona uma nova planta a uma plantação existente.
   * @param plantationId ID da plantação
   * @param plantData Dados da planta (ID e quantidade)
   * @returns Observable com o resultado da operação
   */
  addPlantToPlantation(plantationId: number, plantData: { plantInfoId: number; quantity: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/${plantationId}/add-plant`, plantData);
  }

  /**
   * Remove uma quantidade específica de uma planta de uma plantação.
   * @param plantationId ID da plantação
   * @param plantInfoId ID da planta a remover
   * @param quantity Quantidade a remover
   * @returns Observable com o resultado da operação
   */
  removePlantFromPlantation(plantationId: number, plantInfoId: number, quantity: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${plantationId}/remove-plant/${plantInfoId}`, {
      body: { quantity }
    });
  }

  /**
   * Altera a localização de uma plantação.
   * @param id ID da plantação
   * @param newLocation Nova localização
   * @returns Observable com a plantação atualizada
   */
  updateLocation(id: number, newLocation: Location): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, { location: newLocation });
  }

  /**
   * Utiliza um nível guardado (banked level-up) numa plantação.
   * @param id ID da plantação
   * @returns Observable com a plantação atualizada
   */
  usePlantationBankedLevelUp(id: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${id}/use-banked-levelup`, {});
  }

  /**
   * Ganha experiência numa plantação através de rega ou colheita.
   * @param id ID da plantação
   * @param plantInfoId ID da planta
   * @param isHarvesting Indica se a ação é uma colheita (true) ou rega (false)
   * @returns Observable com resultado da operação
   */
  gainExperience(id: number, plantInfoId: number, isHarvesting: boolean): Observable<any>
  {
    return this.http.put(`${this.apiUrl}/${id}/gain-xp/${plantInfoId}`, isHarvesting);
  }

  /**
   * Elimina uma plantação específica.
   * @param id ID da plantação
   * @returns Observable com confirmação de eliminação
   */
  deletePlantation(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  /**
   * Obtém os dados de uma planta específica dentro de uma plantação.
   * @param plantationId ID da plantação
   * @param plantInfoId ID da planta
   * @returns Observable com dados da planta na plantação
   */
  getPlantationPlantById(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${plantationId}/plant/${plantInfoId}`);
  }

  /**
   * Rega uma planta específica de uma plantação.
   * @param plantationId ID da plantação
   * @param plantInfoId ID da planta
   * @returns Observable com o estado atualizado da planta
   */
  waterPlant(plantationId: number, plantInfoId: number): Observable<PlantationPlant> {
    return this.http.post<PlantationPlant>(
      `${this.apiUrl}/${plantationId}/water-plant/${plantInfoId}`, {});
  }

  /**
   * Verifica se uma planta está pronta para ser colhida e o tempo restante.
   * @param plantationId ID da plantação
   * @param plantInfoId ID da planta
   * @returns Observable com dados sobre a colheita
   */
  checkHarvest(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/${plantationId}/plant/${plantInfoId}/check-harvest`
    );
  }

  /**
   * Colhe uma planta (se estiver pronta).
   * A ação pode desencadear uma nova data de colheita (caso seja recorrente).
   * @param plantationId ID da plantação
   * @param plantInfoId ID da planta
   * @returns Observable com dados da colheita
   */
  harvestPlant(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/${plantationId}/harvest-plant/${plantInfoId}`, {}
    );
  }

}
