import { PlantType } from './plant-type.model';
import { PlantationPlant } from "./plantation-plant";
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
  plantationPlants: PlantationPlant[];
}
