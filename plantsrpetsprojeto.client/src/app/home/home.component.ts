import { Component, OnInit } from '@angular/core';
import { RecentActivityService } from '../recent-activity.service';
import { AuthorizeService } from '../authorize.service';
import { filter } from 'rxjs/operators';

@Component({
  standalone: false,
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  recentPets: string[] = [];
  recentPlantations: string[] = [];

  constructor(
    private recentActivityService: RecentActivityService,
    private authService: AuthorizeService
  ) { }

  ngOnInit(): void {
    this.authService.getUserProfile()
      .pipe(filter(profile => !!profile.profile.userId))
      .subscribe(() => {
        this.loadRecentItems();
      });
  }

  private loadRecentItems(): void {
    this.recentPets = this.recentActivityService.getRecentPets();
    this.recentPlantations = this.recentActivityService.getRecentPlantations();
  }
}
