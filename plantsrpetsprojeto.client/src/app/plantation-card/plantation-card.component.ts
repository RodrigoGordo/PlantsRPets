import { Component, Input, Output, EventEmitter } from '@angular/core';
import { PlantationsService } from '../plantations.service';
import { MatDialog } from '@angular/material/dialog';
import { RemovePlantationPopupComponent } from '../remove-plantation-popup/remove-plantation-popup.component';

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

  constructor(
    private plantationsService: PlantationsService,
    private dialog: MatDialog
  ) { }

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
    const dialogRef = this.dialog.open(RemovePlantationPopupComponent, {
      width: '360px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.plantationsService.deletePlantation(this.plantation.plantationId)
          .subscribe(() => this.deleted.emit());
      }
    });
  }
}
