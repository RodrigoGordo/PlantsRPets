import { Component, OnInit } from '@angular/core';
import { WeatherService } from "../weather.service";

@Component({
  selector: 'app-weather',
  standalone: false,
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  weather: any;
  locationError: string = '';
  isCelsius: boolean = true;

  constructor(private weatherService: WeatherService) { }

  ngOnInit() {
    this.getUserLocation();
  }

  getUserLocation() {
    if ("geolocation" in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const { latitude, longitude } = position.coords;
          this.getWeatherByCoords(latitude, longitude);
        },
        (error) => {
          this.locationError = "Unable to retrieve location. Using default city.";
          this.getWeatherByCity("Setúbal");
        }
      );
    } else {
      this.locationError = "Geolocation is not supported by this browser.";
      this.getWeatherByCity("Setúbal");
    }
  }

  getWeatherByCoords(lat: number, lon: number) {
    this.weatherService.getWeatherByCoords(lat, lon).subscribe(
      (data) => {
        this.weather = data;
      },
      (error) => {
        this.locationError = "Could not retrieve weather data.";
      }
    );
  }

  getWeatherByCity(city: string) {
    this.weatherService.getWeatherByCity(city).subscribe(
      (data) => {
        this.weather = data;
      },
      (error) => {
        this.locationError = "Could not retrieve weather data.";
      }
    );
  }

  toggleUnit() {
    this.isCelsius = !this.isCelsius;
  }
}
