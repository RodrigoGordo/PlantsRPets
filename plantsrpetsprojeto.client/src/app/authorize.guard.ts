import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { Observable, map } from "rxjs";
import { AuthorizeService } from "./authorize.service";

@Injectable({ providedIn: 'root' })
// protects routes from unauthenticated users
export class AuthGuard implements CanActivate  {
  constructor(private authService: AuthorizeService, private router: Router) { }

  canActivate(): boolean {
    const isSignedIn = this.authService.isSignedIn();
    if (!isSignedIn) {
      this.router.navigate(['signin']);
    }
    return isSignedIn;
  }
}
