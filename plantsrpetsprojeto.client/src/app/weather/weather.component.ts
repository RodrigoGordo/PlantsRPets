import { Component, Input, OnInit } from '@angular/core';
import { WeatherService } from "../weather.service";
import { Location } from '../models/location.model';

@Component({
  selector: 'app-weather',
  standalone: false,
  templateUrl: './weather.component.html',
  styleUrls: ['./weather.component.css']
})

/**
 * Componente responsável por apresentar informações meteorológicas atuais
 * e previsões para os próximos dias, com base na localização do utilizador.
 */
export class WeatherComponent implements OnInit {
  @Input() location!: Location;
  weather: any;
  forecast: any[] = [];
  locationError: string = '';
  isCelsius: boolean = true;

  /**
   * Injeta o serviço meteorológico para consumir a API backend.
   * @param weatherService Serviço para obtenção de dados meteorológicos
   */
  constructor(private weatherService: WeatherService) { }

  /**
   * Ciclo de vida do componente. Ao iniciar, tenta obter a localização do utilizador.
   */
  ngOnInit() {
    if (this.location && this.location.latitude && this.location.longitude) {
      this.getWeatherByCoords(this.location.latitude, this.location.longitude);
    }
    else {
      this.getUserLocation();
    }
  }

  /**
   * Obtém a localização do utilizador através do navegador.
   * Se bem-sucedido, chama a API com as coordenadas.
   * Se falhar, usa uma cidade por defeito (Setúbal).
   */
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

  /**
   * Obtém os dados meteorológicos para coordenadas específicas (lat/lon).
   * @param lat Latitude
   * @param lon Longitude
   */
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

  /**
   * Obtém os dados meteorológicos para uma cidade específica.
   * Usado como fallback ou quando o utilizador pesquisa diretamente.
   * @param city Nome da cidade (ex: "Lisboa")
   */
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

  /**
   * Alterna a unidade de temperatura entre Celsius e Fahrenheit.
   * Apenas afeta a apresentação (visual), não a lógica dos dados.
   */
  toggleUnit() {
    this.isCelsius = !this.isCelsius;
  }
}
