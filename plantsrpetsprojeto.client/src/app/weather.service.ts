import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  constructor(private http: HttpClient) { }

  // Fetch weather and forecast by city
  getWeatherByCity(city: string): Observable<any> {
    return this.http.get(`/api/weather/city/${city}`);
  }

  // Fetch weather and forecast by coordinates
  getWeatherByCoords(lat: number, lon: number): Observable<any> {
    return this.http.get(`/api/weather/coords/${lat}/${lon}`);
  }
}
