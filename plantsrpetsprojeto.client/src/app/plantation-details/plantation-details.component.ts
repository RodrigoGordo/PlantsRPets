import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { Plantation } from '../models/plantation.model';

@Component({
  selector: 'app-plantation-details',
  standalone: false,
  
  templateUrl: './plantation-details.component.html',
  styleUrl: './plantation-details.component.css'
})
export class PlantationDetailsComponent implements OnInit {
  plantation!: Plantation;
  loading: boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private plantationsService: PlantationsService
  ) { }

  ngOnInit(): void {
    this.loadPlantation();
  }

  loadPlantation(): void {
    const plantationId = Number(this.route.snapshot.paramMap.get('id'));
    if (!plantationId) {
      this.errorMessage = "Invalid Plantation ID";
      this.loading = false;
      return;
    }

    this.plantationsService.getPlantationById(plantationId).subscribe({
      next: (data) => {
        this.plantation = data;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = "Failed to load plantation details.";
        this.loading = false;
      }
    });
  }
}
