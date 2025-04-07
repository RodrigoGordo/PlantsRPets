/**
 * Representa o perfil de um utilizador na aplicação, contendo dados pessoais e preferências.
 */
export interface UserProfile {
  nickname: string;
  profile: {
    bio: string;
    profilePicture: string | null;
    favoritePets: number[];
    highlightedPlantations: number[];
    profileId: number;
    userId: string;
  };
}
