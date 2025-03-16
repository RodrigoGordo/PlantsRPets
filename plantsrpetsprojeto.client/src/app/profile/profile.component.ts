// profile.component.ts
import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';

@Component({
  standalone: false,
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  nickname: string | null = null;
  profile: any | null = null;

  constructor(private authService: AuthorizeService) { }

  ngOnInit(): void {
    this.authService.getUserProfile().subscribe({
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
