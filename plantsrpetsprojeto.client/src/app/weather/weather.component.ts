import { Component, Input, OnInit } from '@angular/core';
import { WeatherService } from "../weather.service";
import { Location } from '../models/location.model';

@Component({
  selector: 'app-weather',
  standalone: false,
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})
export class WeatherComponent implements OnInit {
  @Input() location!: Location;
  weather: any;
  forecast: any[] = [];
  locationError: string = '';
  isCelsius: boolean = true;

  constructor(private weatherService: WeatherService) { }

  ngOnInit() {
    console.log("Localização do Weather Component");
    console.log(this.location);
    this.getUserLocation();
    this.getWeatherByCoords(this.location.latitude, this.location.longitude);
  }

  getUserLocation() {
    if ("geolocation" in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const { latitude, longitude } = position.coords;
          this.getWeatherByCoords(latitude, longitude);
          this.locationError = "";
        },
        (error) => {
          this.locationError = "Allow location permission in browser to display the weather";
        }
      );
    } else {
      this.locationError = "Geolocation is not supported by this browser.";
    }
  }

  getWeatherByCoords(lat: number, lon: number) {
    this.weatherService.getWeatherByCoords(lat, lon).subscribe(
      (data) => {
        this.weather = data;
        console.log("Weather Data");
        console.log(this.weather);
        this.forecast = data.forecast.forecastday;
        this.locationError = "";
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
        this.forecast = data.forecast.forecastday;
        this.locationError = ""
      },
      (error) => {
        if (!this.weather) {
          this.locationError = "Please specify the location of your plantation to display the weather!";
        } else {
          this.locationError = "Could not retrieve weather data.";
        }
      }
    );
  }

  toggleUnit() {
    this.isCelsius = !this.isCelsius;
  }
}
