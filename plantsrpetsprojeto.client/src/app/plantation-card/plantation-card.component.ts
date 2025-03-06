import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PlantationsService } from '../plantations.service';

@Component({
  selector: 'app-plantation-card',
  standalone: false,
  
  templateUrl: './plantation-card.component.html',
  styleUrl: './plantation-card.component.css'
})

export class PlantationCardComponent {
  @Input() plantation: any;
  @Output() updated = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<void>();

  isEditing: boolean = false;
  newPlantationName: string = '';

  constructor(private plantationsService: PlantationsService) { }

  enableEdit(): void {
    this.isEditing = true;
    this.newPlantationName = this.plantation.plantationName;
  }

  saveEdit(): void {
    if (this.newPlantationName.trim() === '') return;

    this.plantationsService.updatePlantationName(this.plantation.plantationId, this.newPlantationName)
      .subscribe(() => {
        this.plantation.plantationName = this.newPlantationName;
        this.isEditing = false;
        this.updated.emit();
      });
  }

  cancelEdit(): void {
    this.isEditing = false;
  }

  remove(): void {
    if (confirm(`Are you sure you want to delete "${this.plantation.plantationName}"?`)) {
      this.plantationsService.deletePlantation(this.plantation.plantationId)
        .subscribe(() => this.deleted.emit());
    }
  }
}
