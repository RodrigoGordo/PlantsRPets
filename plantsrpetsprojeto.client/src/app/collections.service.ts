import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CollectionService {
  private apiUrl = 'api/collections';

  constructor(private http: HttpClient) { }

  // Buscar 3 pets random não-owned
  getRandomUnownedPets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/random-unowned`);
  }

  // Atualizar propriedade IsOwned
  updateOwnedStatus(petId: number, isOwned: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/owned/${petId}`, { isOwned });
  }

  getFavoritePetsInCollection(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/favoritePets`);
  }
}
