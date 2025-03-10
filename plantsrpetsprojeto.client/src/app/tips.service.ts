import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tip } from '../app/models/tip-model';

@Injectable({
  providedIn: 'root'
})
export class TipService {
  private apiUrl = 'api/SustainabilityTips'; // Replace with your API endpoint

  constructor(private http: HttpClient) { }

  getTipsByPlantId(plantInfoId: number): Observable<Tip[]> {
    return this.http.get<Tip[]>(`${this.apiUrl}/by-plant/${plantInfoId}`);
  }
}
