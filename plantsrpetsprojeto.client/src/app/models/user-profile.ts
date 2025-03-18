export interface UserProfile {
  nickname: string;
  profile: {
    bio: string;
    profilePictureUrl: string | null;
    favoritePets: number[];
    highlightedPlantations: number[];
    profileId: number;
    userId: string;
  };
}
