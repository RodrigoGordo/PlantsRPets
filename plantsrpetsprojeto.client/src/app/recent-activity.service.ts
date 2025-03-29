import { Injectable } from '@angular/core';
import { AuthorizeService } from './authorize.service';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
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

  private getUserStorageKey(prefix: string): string | null {
    if (!this.userId) {
      return null;
    }
    return `${prefix}${this.userId}`;
  }

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

  private saveRecentItem(storageKey: string | null, item: string): void {
    if (!storageKey) return;
    let items = this.getRecentItems(storageKey);
    items = items.filter((i: string) => i !== item);
    items.unshift(item);
    items = items.slice(0, 3);
    localStorage.setItem(storageKey, JSON.stringify(items));
  }

  private removeItem(storageKey: string | null, itemId: string): void {
    if (!storageKey) return;
    let items = this.getRecentItems(storageKey);
    items = items.filter((i: string) => i !== itemId);
    localStorage.setItem(storageKey, JSON.stringify(items));
  }

  savePet(petId: string): void {
    if (!this.userId) {
      return;
    }
    this.saveRecentItem(this.getUserStorageKey(this.PETS_STORAGE_PREFIX), petId);
  }

  getRecentPets(): string[] {
    if (!this.userId) {
      return [];
    }
    return this.getRecentItems(this.getUserStorageKey(this.PETS_STORAGE_PREFIX));
  }

  removePet(petId: string): void {
    this.removeItem(this.getUserStorageKey(this.PETS_STORAGE_PREFIX), petId);
  }

  removeAllPets(): void {
    const storageKey = this.getUserStorageKey(this.PETS_STORAGE_PREFIX);
    if (storageKey) {
      localStorage.setItem(storageKey, JSON.stringify([]));
    }
  }

  savePlantation(plantationId: string): void {
    if (!this.userId) {
      return;
    }
    this.saveRecentItem(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX), plantationId);
  }

  getRecentPlantations(): string[] {
    if (!this.userId) {
      return [];
    }
    return this.getRecentItems(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX));
  }

  removePlantation(plantationId: string): void {
    this.removeItem(this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX), plantationId);
  }

  removeAllPlantations(): void {
    const storageKey = this.getUserStorageKey(this.PLANTATIONS_STORAGE_PREFIX);
    if (storageKey) {
      localStorage.setItem(storageKey, JSON.stringify([]));
    }
  }

  removeAllUsersData(): void {
    Object.keys(localStorage).forEach((key) => {
      if (key.startsWith(this.PETS_STORAGE_PREFIX) || key.startsWith(this.PLANTATIONS_STORAGE_PREFIX)) {
        localStorage.removeItem(key);
      }
    });
  }

}
