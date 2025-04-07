import { Injectable } from '@angular/core';
import { AuthorizeService } from './authorize.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço para armazenar e recuperar os últimos itens visualizados (animais de estimação e plantações).
 * Os dados são guardados no armazenamento local (localStorage) e associados ao utilizador autenticado.
 */
export class RecentActivityService {
  private readonly PETS_STORAGE_PREFIX = 'recentPets_';
  private readonly PLANTATIONS_STORAGE_PREFIX = 'recentPlantations_';
  private userId: string | null = null;

  constructor(private authService: AuthorizeService) {
    this.authService.getUserProfile()
      .pipe(filter(profile => !!profile.profile.userId))
      .subscribe(profile => {
        this.userId = profile.profile.userId;
      });
  }

  /**
   * Constrói a chave de armazenamento local específica do utilizador.
   * @param prefix Prefixo da chave (ex: 'recentPets_' ou 'recentPlantations_')
   * @returns A chave única para o utilizador atual, ou null se não autenticado
   */
  private getUserStorageKey(prefix: string): string | null {
    if (!this.userId) {
      return null;
    }
    return `${prefix}${this.userId}`;
  }

  /**
   * Obtém os itens recentes guardados localmente com base na chave.
   * @param storageKey Chave de armazenamento
   * @returns Array de strings com os IDs dos itens recentes
   */
  private getRecentItems(storageKey: string | null): string[] {
    if (!storageKey) return [];
    try {
      const items = localStorage.getItem(storageKey);
      return items ? JSON.parse(items) : [];
    } catch (error) {
      console.error(`Error parsing ${storageKey}`, error);
      return [];
    }
  }

  /**
   * Guarda um novo item na lista de itens recentes (no topo da lista).
   * Evita duplicados e limita o número de itens a 3.
   * @param storageKey Chave do armazenamento
   * @param item ID do item a guardar
   */
  private saveRecentItem(storageKey: string | null, item: string): void {
    if (!storageKey) return;
    let items = this.getRecentItems(storageKey);
    items = items.filter((i: string) => i !== item);
    items.unshift(item);
    items = items.slice(0, 3);
    localStorage.setItem(storageKey, JSON.stringify(items));
  }

  /**
   * Remove um item específico do armazenamento recente.
   * @param storageKey Chave do armazenamento
   * @param itemId ID do item a remover
   */
  private removeItem(storageKey: string | null, itemId: string): void {
    if (!storageKey) return;
    let items = this.getRecentItems(storageKey);
    items = items.filter((i: string) => i !== itemId);
    localStorage.setItem(storageKey, JSON.stringify(items));
  }

  /**
   * Adiciona um animal à lista de animais visualizados recentemente.
   * @param petId ID do animal
   */
  savePet(petId: string): void {
    if (!this.userId) {
      return;
    }
    this.saveRecentItem(this.getUserStorageKey(this.PETS_STORAGE_PREFIX), petId);
  }

  /**
   * Obtém os animais visualizados recentemente.
   * @returns Lista de IDs dos animais
   */
  getRecentPets(): string[] {
    if (!this.userId) {
      return [];
    }
    return this.getRecentItems(this.getUserStorageKey(this.PETS_STORAGE_PREFIX));
  }

  /**
   * Remove um animal específico da lista de recentes.
   * @param petId ID do animal a remover
   */
  removePet(petId: string): void {
    this.removeItem(this.getUserStorageKey(this.PETS_STORAGE_PREFIX), petId);
  }

  /**
   * Remove todos os animais da lista de recentes.
   */
  removeAllPets(): void {
    const storageKey = this.getUserStorageKey(this.PETS_STORAGE_PREFIX);
    if (storageKey) {
      localStorage.setItem(storageKey, JSON.stringify([]));
    }
  }

  /**
   * Adiciona uma plantação à lista de plantações visualizadas recentemente.
   * @param plantationId ID da plantação
   */
  savePlantation(plantationId: string): void {
    if (!this.userId) {
      return;
    }
    this.saveRecentItem(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX), plantationId);
  }

  /**
   * Obtém as plantações visualizadas recentemente.
   * @returns Lista de IDs das plantações
   */
  getRecentPlantations(): string[] {
    if (!this.userId) {
      return [];
    }
    return this.getRecentItems(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX));
  }

  /**
   * Remove uma plantação específica da lista de recentes.
   * @param plantationId ID da plantação
   */
  removePlantation(plantationId: string): void {
    this.removeItem(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX), plantationId);
  }

  /**
   * Remove todas as plantações da lista de recentes.
   */
  removeAllPlantations(): void {
    const storageKey = this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX);
    if (storageKey) {
      localStorage.setItem(storageKey, JSON.stringify([]));
    }
  }

  /**
   * Remove todos os dados de animais e plantações de todos os utilizadores do localStorage.
   */
  removeAllUsersData(): void {
    Object.keys(localStorage).forEach((key) => {
      if (key.startsWith(this.PETS_STORAGE_PREFIX) || key.startsWith(this.PLANTATIONS_STORAGE_PREFIX)) {
        localStorage.removeItem(key);
      }
    });
  }

}
