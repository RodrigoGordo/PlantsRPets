import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

/**
 * Componente responsável pela gestão e visualização da coleção de pets do utilizador.
 * Este componente pode ser utilizado para exibir as coleções de pets colecionáveis da aplicação.
 */
@Component({
  selector: 'app-collection',
  standalone: false,
  
  templateUrl: './collection.component.html',
  styleUrl: './collection.component.css'
})
export class CollectionComponent {
  pets: any[] = [];

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any[]>('/api/pets').subscribe(data => {
      this.pets = data;
    });
  }

}
