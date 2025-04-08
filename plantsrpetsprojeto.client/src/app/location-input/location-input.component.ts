import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CityService } from '../city.service';
import { Location } from '../models/location.model';

@Component({
  selector: 'app-location-input',
  standalone: false,
  
  templateUrl: './location-input.component.html',
  styleUrl: './location-input.component.css'
})

/**
 * Componente responsável por pesquisar e selecionar localizações (cidade, região, país) com base em input do utilizador.
 * Emite o objeto `Location` quando uma cidade é selecionada.
 */
export class LocationInputComponent {
  @Output() locationSelected = new EventEmitter<Location>();

  citySearchTerm: string = '';
  citySearchResults: any[] = [];
  private searchDebounceTimer: any;


  /**
   * Construtor que injeta o serviço responsável por buscar dados de cidades.
   * @param cityService Serviço para obter cidades a partir do nome.
   */
  constructor(private cityService: CityService) { }

  /**
   * Método chamado sempre que o utilizador escreve no campo de pesquisa.
   * Aguarda 1 segundo após o último input para enviar a requisição (debounce).
   */
  onCitySearchInput(): void {
    clearTimeout(this.searchDebounceTimer);
    this.searchDebounceTimer = setTimeout(() => {
      if (this.citySearchTerm.trim()) {
        this.cityService.getCitiesByName(this.citySearchTerm).subscribe({
          next: (cities) => this.citySearchResults = cities,
          error: (err) => {
            console.error('Error fetching cities:', err);
            this.citySearchResults = [];
          }
        });
      } else {
        this.citySearchResults = [];
      }
    }, 1000);
  }

  /**
   * Método chamado quando o utilizador seleciona uma cidade da lista de sugestões.
   * Constrói o objeto `Location` e emite o evento `locationSelected`.
   * 
   * @param cityData Dados da cidade selecionada (nome, região, país, coordenadas).
   */
  selectCity(cityData: any): void {
    const newLocation: Location = {
      city: cityData.name,
      region: cityData.region,
      country: cityData.country,
      latitude: cityData.lat,
      longitude: cityData.lon
    };

    this.locationSelected.emit(newLocation);
    this.citySearchTerm = `${cityData.name}, ${cityData.region}, ${cityData.country}`;
    this.citySearchResults = [];
  }
}
