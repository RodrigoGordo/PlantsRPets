import { Component, HostListener, OnInit } from '@angular/core';
import { PlantsService } from '../plants.service';
import { PlantInfo } from '../models/plant-info';

@Component({
  selector: 'app-wiki',
  standalone: false,
  templateUrl: './wiki.component.html',
  styleUrls: ['./wiki.component.css']
})

/**
 * Componente responsável por apresentar a "wiki" das plantas disponíveis.
 * Permite pesquisa por nome e filtragem por atributos (ex: exposição solar, tipo, etc.).
 */export class WikiComponent implements OnInit {
  plants: PlantInfo[] = [];
  searchQuery = '';
  activeFilters: { [key: string]: any } = {};
  showFilters = false;

  displayCount = 16;
  hasMore = true;

  /**
   * Injeta o serviço responsável por buscar os dados das plantas.
   * @param plantService Serviço que fornece os dados de plantas via API
   */
  constructor(private plantService: PlantsService) { }

  /**
   * No carregamento do componente, obtém a lista de plantas da API.
   */
  ngOnInit(): void {
    this.loadInitialPlants();
  }

  /**
   * Carrega a lista inicial de plantas da API.
   */
  private loadInitialPlants(): void {
    this.plantService.getPlants().subscribe(plants => {
      this.plants = plants;
      this.updateDisplayedPlants();
    });
  }

  /**
   * Retorna a lista de plantas filtradas com base na pesquisa e nos filtros ativos.
   */
  get filteredPlants(): PlantInfo[] {
    return this.plants.filter(plant =>
      plant.plantName.toLowerCase().includes(this.searchQuery.toLowerCase()) &&
      this.matchesFilters(plant)
    );
  }

  /**
   * Verifica se uma planta cumpre todos os filtros ativos.
   * @param plant A planta a verificar
   */
  get displayedPlants(): PlantInfo[] {
    return this.filteredPlants.slice(0, this.displayCount);
  }

  /**
   * Deteta scroll na janela para ativar carregamento adicional de plantas.
   */
  @HostListener('window:scroll', ['$event'])
  onScroll(): void {
    if (this.hasMore && this.isNearBottom()) {
      this.loadMorePlants();
    }
  }

  /**
   * Verifica se o utilizador está perto do final da página.
   */
  private isNearBottom(): boolean {
    const threshold = 100;
    const position = window.scrollY + window.innerHeight;
    const height = document.body.scrollHeight;
    return position > height - threshold;
  }

  /**
   * Aumenta o número de plantas visíveis caso existam mais disponíveis.
   */
  private loadMorePlants(): void {
    if (this.displayCount < this.filteredPlants.length) {
      this.displayCount += 16;
      this.hasMore = this.displayCount < this.filteredPlants.length;
    }
  }

  /**
   * Atualiza o número de plantas visíveis após pesquisa ou alteração de filtros.
   */
  private updateDisplayedPlants(): void {
    this.displayCount = 16;
    this.hasMore = this.filteredPlants.length > this.displayCount;
  }

  /**
   * Verifica se uma planta cumpre todos os filtros ativos.
   * @param plant Planta a verificar
   * @returns true se todos os filtros forem satisfeitos
   */
  private matchesFilters(plant: PlantInfo): boolean {
    return Object.keys(this.activeFilters).every(key => {
      const filterValue = this.activeFilters[key];
      if (!filterValue) return true;

      const plantKey = key as keyof PlantInfo;
      const plantValue = plant[plantKey];

      if (['edible', 'fruits'].includes(plantKey)) {
        const plantBool = this.convertYesNoToBoolean(plantValue);
        const filterBool = filterValue === 'true';
        return plantBool === filterBool;
      }

      if (typeof plantValue === 'boolean') {
        return plantValue === (filterValue === 'true');
      }

      if (plantKey === 'sunlight') {
        const normalizedFilter = filterValue.toLowerCase()
          .replace(/[-\/]/g, ' ')
          .replace(/\s+/g, ' ')
          .trim();

        const plantSunlight = plant.sunlight || [];
        return plantSunlight.some(s => {
          const normalizedPlant = (s || '')
            .toLowerCase()
            .replace(/[-\/]/g, ' ')
            .replace(/\s+/g, ' ')
            .trim();

          return normalizedPlant === normalizedFilter;
        });
      }

      if (plantValue == null) return false;

      return plantValue.toString().toLowerCase().trim() ===
        filterValue.toString().toLowerCase().trim();
    });
  }

  /**
   * Converte valores "yes"/"no" em booleanos.
   * @param value Valor textual ou booleano
   * @returns true se for "yes" ou boolean true
   */
  private convertYesNoToBoolean(value: any): boolean {
    if (typeof value === 'boolean') return value;
    if (typeof value === 'string') {
      return value.toLowerCase() === 'yes';
    }
    return false;
  }

  /**
   * Atualiza os filtros ativos quando o utilizador altera os filtros no painel.
   * @param filters Objeto com os filtros atualizados
   */
  onFiltersChanged(filters: any): void {
    this.activeFilters = filters;
    this.updateDisplayedPlants();
  }

  /**
   * Alterna a visibilidade do painel de filtros.
   */
  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  /**
   * Fecha o painel de filtros (ex: ao clicar fora).
   */
  onFiltersClosed(): void {
    this.showFilters = false;
  }
}
