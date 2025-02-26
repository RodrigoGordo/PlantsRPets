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
  @Output() deleted = new EventEmitter<void>();

  constructor(private plantationsService: PlantationsService) { }

  remove(): void {
    if (confirm(`Are you sure you want to delete "${this.plantation.plantationName}"?`)) {
      this.plantationsService.deletePlantation(this.plantation.plantationId).subscribe(() => {
        this.deleted.emit();  // Atualiza a lista após a remoção
      });
    }
  }
}
