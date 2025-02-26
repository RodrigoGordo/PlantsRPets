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
  city = 'Setúbal';

  constructor(private weatherService: WeatherService) { }

  ngOnInit() {
    this.getWeather();
  }

  getWeather() {
    this.weatherService.getWeather(this.city).subscribe(data => {
      console.log(data); // Log the response to the console
      this.weather = data;
    });
  }
}
