import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-navbar',
  standalone: false,

  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  isSignedIn: boolean = false;

  constructor(
    public dialog: MatDialog,
    private authService: AuthorizeService
  ) {
    this.authService.onStateChanged().subscribe((state: boolean) => {
      this.isSignedIn = state;
    });
  }
}
