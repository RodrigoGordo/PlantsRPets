import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TipService } from '../tips.service';
import { Tip } from '../models/tip-model';

@Component({
  selector: 'app-plant-info-page',
  standalone: false,
  templateUrl: './plant-info-page.component.html',
  styleUrls: ['./plant-info-page.component.css']
})
export class PlantInfoPageComponent implements OnInit {
  plantInfoId!: number; // Plant ID to fetch tips for
  tips: Tip[] = []; // List of tips
  isLoading = true; // Loading state
  errorMessage: string | null = null; // Error message

  constructor(
    private route: ActivatedRoute, // To get route parameters
    private tipService: TipService // Service to fetch tips
  ) { }

  ngOnInit(): void {
    this.plantInfoId = +this.route.snapshot.paramMap.get("id")!;
    this.fetchTips();
  }

  private fetchTips(): void {
    this.tipService.getTipsByPlantId(this.plantInfoId).subscribe({
      next: (data) => {
        this.tips = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load tips. Please try again later.';
        this.isLoading = false;
      }
    });
  }
}
