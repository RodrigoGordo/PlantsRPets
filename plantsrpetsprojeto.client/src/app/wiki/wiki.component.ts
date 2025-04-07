import { Component, OnInit } from '@angular/core';
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

  /**
   * Injeta o serviço responsável por buscar os dados das plantas.
   * @param plantService Serviço que fornece os dados de plantas via API
   */
  constructor(private plantService: PlantsService) { }

  /**
   * No carregamento do componente, obtém a lista de plantas da API.
   */
  ngOnInit(): void {
    this.plantService.getPlants().subscribe(plants => {
      this.plants = plants;
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
  private matchesFilters(plant: PlantInfo): boolean {
    return Object.keys(this.activeFilters).every(key => {
      const filterValue = this.activeFilters[key];
      if (!filterValue) return true;

      const plantKey = key as keyof PlantInfo;
      const plantValue = plant[plantKey];

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

      if (typeof plantValue === 'boolean') {
        return plantValue === (filterValue === 'true');
      }

      if (plantValue == null) return false;

      return plantValue.toString().toLowerCase().trim() ===
        filterValue.toString().toLowerCase().trim();
    });
  }

  /**
   * Atualiza os filtros ativos quando o utilizador altera os filtros no painel.
   * @param filters Objeto com os filtros atualizados
   */
  onFiltersChanged(filters: any): void {
    this.activeFilters = filters;
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
