import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from './authorize.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthorizeService) { }

  ngOnInit(): void {
    const token = localStorage.getItem('authToken');
    if (token) {
      try {
        const decoded: any = jwtDecode(token);
        const exp = decoded.exp * 1000;
        if (Date.now() >= exp) {
          this.authService.signOut();
          this.authService.requestLoginPopup();
        }
      } catch (error) {
        console.error("Erro ao decodificar token:", error);
        this.authService.signOut();
        this.authService.requestLoginPopup();
      }
    }
  }
}
