import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizeService } from '../authorize.service';

@Component({
  selector: 'app-confirm-email',
  standalone: false,
  
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})

export class ConfirmEmailComponent implements OnInit {
  message: string = "Verifying your email...";
  success: boolean = false;
  isLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthorizeService
  ) { }

  ngOnInit(): void {
    this.isLoading = true;

    const email = this.route.snapshot.queryParamMap.get('email');
    const token = this.route.snapshot.queryParamMap.get('token');

    if (email && token) {
      this.authService.confirmEmail(email, token).subscribe(success => {
        if (success) {
          this.isLoading = false;
          this.message = "Your email has been successfully verified!";
          this.success = true;
        } else {
          this.isLoading = false;
          this.message = "Failed to verify email. The link might be invalid or expired.";
          this.success = false;
        }
      });
    } else {
      this.isLoading = false;
      this.message = "Invalid confirmation link.";
    }
  }

  goHome(): void {
    this.router.navigate(['/']);
  }
}
