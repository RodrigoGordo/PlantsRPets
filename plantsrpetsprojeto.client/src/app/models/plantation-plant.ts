import { PlantInfo } from "./plant-info";

export interface PlantationPlant {
  plantInfoId: number;
  quantity: number;
  referencePlant: PlantInfo;
}
