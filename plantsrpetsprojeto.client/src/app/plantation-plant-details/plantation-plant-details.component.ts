import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlantationsService } from '../plantations.service';
import { PlantationPlant } from '../models/plantation-plant';
import { Location } from '@angular/common';

@Component({
  selector: 'app-plantation-plant-details',
  standalone: false,
  templateUrl: './plantation-plant-details.component.html',
  styleUrls: ['./plantation-plant-details.component.css']
})
export class PlantationPlantDetailsComponent implements OnInit {
  plantInfoId!: number;
  plantationId!: number;

  plantationPlant!: PlantationPlant;

  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private plantationsService: PlantationsService,
    private location: Location
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.plantationId = Number(this.route.snapshot.paramMap.get('plantationId'));
      this.plantInfoId = Number(this.route.snapshot.paramMap.get('plantInfoId'));
    })

    this.loadPlantationPlantDetails();
  }

  loadPlantationPlantDetails() {
    if (!this.plantationId) return;
    if (!this.plantInfoId) return;

    this.isLoading = true;

    this.plantationsService.getPlantationPlantById(this.plantationId, this.plantInfoId).subscribe({
      next: (data) => {
        console.log("Plant received:", data);
        this.plantationPlant = data;
        this.isLoading = false;
      },
      error: () => {
        console.log("Erro no load das plantas - Msg de erro temp");
        this.isLoading = false;
      }
    });
  }

  goBack() {
    this.location.back();
  }
}
