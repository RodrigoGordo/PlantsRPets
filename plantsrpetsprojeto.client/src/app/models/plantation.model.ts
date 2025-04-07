import { PlantType } from './plant-type.model';
import { PlantationPlant } from "./plantation-plant";
import { Location } from './location.model';

/**
 * Representa uma plantação criada pelo utilizador.
 */
export interface Plantation {
  plantationId: number;
  plantationName: string;
  plantTypeId: number;
  plantTypeName: string;
  lastWatered: Date;
  plantingDate: Date;
  harvestDate: Date;
  growthStatus: string;
  experiencePoints: number;
  level: number;
  bankedLevelUps: number;
  location?: Location;
  plantationPlants: PlantationPlant[];
}
