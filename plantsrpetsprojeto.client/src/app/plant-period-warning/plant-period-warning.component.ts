import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface PlantPeriodWarningData {
  message: string;
  idealMonths: string[];
}

@Component({
  selector: 'app-plant-period-warning',
  standalone: false,
  
  templateUrl: './plant-period-warning.component.html',
  styleUrl: './plant-period-warning.component.css'
})

export class PlantPeriodWarningComponent {

  constructor(
    private dialogRef: MatDialogRef<PlantPeriodWarningComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PlantPeriodWarningData
  ) { }

  confirm(): void {
    this.dialogRef.close(true);
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
