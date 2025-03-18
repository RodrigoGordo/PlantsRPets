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
