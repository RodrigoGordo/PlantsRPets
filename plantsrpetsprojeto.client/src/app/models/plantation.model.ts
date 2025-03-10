import { PlantType } from './plant-type.model';
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
}
