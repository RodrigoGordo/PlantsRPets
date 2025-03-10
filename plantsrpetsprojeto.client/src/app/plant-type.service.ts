import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlantTypesService {
  private apiUrl = 'https://localhost:7024/api/plantations';

  constructor(private http: HttpClient) { }

  getPlantTypes(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
