import { PruningCountInfo } from "./pruning-count-info";

/**
 * Representa os dados detalhados de uma planta disponível na base de dados.
 */
export interface PlantInfo {
  plantInfoId: number;
  plantName: string;
  plantType: string;
  cycle: string;
  watering: string;
  pruningMonth: string[];
  pruningCount: PruningCountInfo;
  growthRate: string;
  sunlight: string[]; 
  edible: string;
  careLevel: string;
  flowers: string;
  fruits: string;
  leaf: boolean;
  maintenance: string;
  saltTolerant: string;
  indoor: boolean;
  floweringSeason: string;
  description: string;
  image: string;
  harvestSeason: string;
  scientificName: string[];
  droughtTolerant: boolean;
  cuisine: boolean;
  medicinal: boolean;
}
