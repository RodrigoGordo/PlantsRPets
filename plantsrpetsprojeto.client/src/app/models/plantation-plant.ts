import { PlantInfo } from "./plant-info";

/**
 * Representa uma instância de uma planta dentro de uma plantação específica.
 */
export interface PlantationPlant {
  plantationPlantsId: number;
  plantationId: number;
  plantInfoId: number;
  referencePlant: PlantInfo;
  quantity: number;
  plantingDate: Date;
  lastWatered: Date | null;
  lastHarvested: Date | null;
  harvestDate: Date | null;
  growthStatus: string;
}
