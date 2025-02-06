// login-menu.component.ts
import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SigninComponent } from '../signin/signin.component';
import { AuthorizeService } from '../authorize.service';
import { Router } from '@angular/router';
import { LogoutConfirmationComponent } from '../logout-confirmation/logout-confirmation.component';


@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css'],
  standalone: false,
})
export class LoginMenuComponent {
  isSignedIn: boolean = false;
  dropdownOpen: boolean = false;

  constructor(
    public dialog: MatDialog,
    private authService: AuthorizeService,
    private router: Router
  ) {
    this.authService.onStateChanged().subscribe((state: boolean) => {
      this.isSignedIn = state;
    });
  }

  openSignInDialog(): void {
    this.dialog.open(SigninComponent, {
      width: '520px',
      panelClass: 'custom-dialog-container',
      disableClose: true
    });
  }

  confirmLogout(): void {
    const dialogRef = this.dialog.open(LogoutConfirmationComponent, {
      width: '350px',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'confirm') {
        this.signOut();
      }
    });
  }

  signOut(): void {
    if (this.isSignedIn) {
      this.authService.signOut();
      this.isSignedIn = false;
      this.dropdownOpen = false;
      this.router.navigateByUrl("");
    }
  }

  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }
}
