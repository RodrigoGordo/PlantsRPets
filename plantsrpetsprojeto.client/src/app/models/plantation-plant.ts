import { PlantInfo } from "./plant-info";

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
