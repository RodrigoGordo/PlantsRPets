export interface UserProfile {
  nickname: string | null;
  profile: {
    bio: string;
    profilePictureUrl: string;
    dateOfBirth?: Date;
  } | null;
}
