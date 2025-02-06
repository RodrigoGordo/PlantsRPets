import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-logout-confirmation',
  standalone: false,
  
  templateUrl: './logout-confirmation.component.html',
  styleUrl: './logout-confirmation.component.css'
})
export class LogoutConfirmationComponent {
  constructor(private dialogRef: MatDialogRef<LogoutConfirmationComponent>) { }

  confirm(): void {
    this.dialogRef.close('confirm');
  }

  cancel(): void {
    this.dialogRef.close('cancel');
  }
}
