import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PlantInfo } from './models/plant-info';
import { PlantationPlant } from './models/plantation-plant';
import { Plantation } from './models/plantation.model'; 

@Injectable({
  providedIn: 'root'
})
export class PlantationsService {
  private apiUrl = 'api/plantations';

  constructor(private http: HttpClient) { }

  // Obtém todas as plantações do user
  getUserPlantations(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtém uma plantação específica
  getPlantationById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  // Obtém as plantas de uma plantação
  getPlantsInPlantation(plantationId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/${plantationId}/plants`);
  }

  // Adiciona uma planta a uma plantação
  addPlantToPlantation(plantationId: number, plantData: { plantInfoId: number; quantity: number }): Observable<any> {
    return this.http.post(`${this.apiUrl}/${plantationId}/add-plant`, plantData);
  }

  // Cria uma nova plantação
  createPlantation(plantationData: { plantationName: string; plantTypeId: number }): Observable<any> {
    return this.http.post<any>(this.apiUrl, plantationData);
  }

  // Atualiza uma plantação
  updatePlantationName(id: number, newName: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, { plantationName: newName });
  }

  // Remove uma plantação pelo ID
  deletePlantation(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getPlantationPlantById(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${plantationId}/plant/${plantInfoId}`);
  }

  waterPlant(plantationId: number, plantInfoId: number): Observable<PlantationPlant> {
    return this.http.post<PlantationPlant>(
      `${this.apiUrl}/${plantationId}/water-plant/${plantInfoId}`, {});
  }

  // Verifica quando é a próxima colheita
  checkHarvest(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.get<any>(
      `${this.apiUrl}/${plantationId}/plant/${plantInfoId}/check-harvest`
    );
  }

  // Colhe uma planta (executa a colheita)
  harvestPlant(plantationId: number, plantInfoId: number): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/${plantationId}/harvest-plant/${plantInfoId}`, {}
    );
  }

}
