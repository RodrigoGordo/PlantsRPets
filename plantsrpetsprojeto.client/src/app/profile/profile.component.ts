import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../../services/profile.service';

@Component({
  standalone: false,
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  nickname: string | null = null;
  profile: any = null;

  constructor(private profileService: ProfileService) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (response) => {
        this.nickname = response.nickname;
        this.profile = response.profile;
      },
      error: (err) => {
        console.error('Failed to fetch profile:', err);
      }
    });
  }
}
