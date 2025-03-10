import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlantationsService {
  private apiUrl = 'https://localhost:7024/api/plantations';

  constructor(private http: HttpClient) { }

  // Obtém todas as plantações do user
  getUserPlantations(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Obtém uma plantação específica
  getPlantationById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
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
}
