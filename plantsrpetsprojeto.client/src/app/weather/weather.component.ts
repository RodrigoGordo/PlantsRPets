import { Component, OnInit } from '@angular/core';
import { WeatherService } from "../weather.service";

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
    this.getUserLocation();
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

  /**
   * Obtém os dados meteorológicos para coordenadas específicas (lat/lon).
   * @param lat Latitude
   * @param lon Longitude
   */
  getWeatherByCoords(lat: number, lon: number) {
    this.weatherService.getWeatherByCoords(lat, lon).subscribe(
      (data) => {
        this.weather = data;
        this.forecast = data.forecast.forecastday;
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
      },
      (error) => {
        this.locationError = "Could not retrieve weather data.";
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
