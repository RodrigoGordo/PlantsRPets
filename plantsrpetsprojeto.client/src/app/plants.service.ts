import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlantInfo } from './models/plant-info';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlantsService {
  private apiUrl = 'api/plants';

  constructor(private http: HttpClient) { }

  getPlants(): Observable<PlantInfo[]> { 
    return this.http.get<PlantInfo[]>(this.apiUrl);
  }

  getPlantById(id: number): Observable<PlantInfo> {
    return this.http.get<PlantInfo>(`${this.apiUrl}/${id}`);
  }

  getPlantingPeriodCheck(id: number): Observable<any> {
    return this.http.get<any>(`api/plants/check-planting-period/${id}`);
  }
}
