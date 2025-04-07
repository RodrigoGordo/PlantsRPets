/**
 * Representa um pet colecionável, gerado e atribuído ao utilizador.
 */
export interface Pet {
  petId: number;
  name: string;
  type: string;
  details: string;
  battleStats: string;
  imageUrl: string;
  isOwned: boolean;
  isFavorite: boolean;
}
