import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Pet } from '../models/pet';

/**
 * Componente responsável pela gestão e visualização da coleção de pets do utilizador.
 * Este componente exibe as coleções de pets colecionáveis da aplicação, mostrando quais o utilizador possui.
 */
@Component({
  selector: 'app-collection',
  standalone: false,
  templateUrl: './collection.component.html',
  styleUrl: './collection.component.css'
})
export class CollectionComponent implements OnInit {
  pets: Pet[] = [];
  loading: boolean = true;
  error: string | null = null;

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadUserCollection();
  }

  loadUserCollection(): void {
    this.loading = true;
    this.http.get<Pet[]>('/api/collections').subscribe({
      next: (data) => {
        this.pets = data.sort((a, b) => {
          if (b.isOwned !== a.isOwned) {
            return Number(b.isOwned) - Number(a.isOwned); // Owned vem primeiro
          }
          return Number(b.isFavorite) - Number(a.isFavorite); // Dentro de owned, favoritos primeiro
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load your collection. Please try again later.';
        this.loading = false;
        console.error('Error loading collection:', err);
      }
    });
  }

}
