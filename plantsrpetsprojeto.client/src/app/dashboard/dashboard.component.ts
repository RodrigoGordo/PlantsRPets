import { Component, OnInit } from '@angular/core';
import { MetricsService } from '../metrics.service';
import { Color, ScaleType } from '@swimlane/ngx-charts';


@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})


/**
 * Componente responsável por apresentar métricas de atividades do utilizador no formato de gráficos.
 * Inclui contagem de eventos (rega, colheita, plantação) ao longo do tempo e distribuição por tipo de planta.
 */
export class DashboardComponent implements OnInit {
  timeFrame: string = 'week';
  activityCounts: any = {};

  wateringData: any[] = [];
  harvestData: any[] = [];
  plantingData: any[] = [];
  plantTypeData: any[] = [];

  view: [number, number] = [700, 400];

  colorScheme: Color = {
    name: 'custom',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a27ea8', '#aae3f5']
  };


  showXAxis = true;
  showYAxis = true;
  showLegend = true;
  showXAxisLabel = true;
  showYAxisLabel = true;
  xAxisLabel = 'Date';
  yAxisLabel = 'Count';

  isLoading = true;

  /**
   * Injeta o serviço de métricas responsável por obter os dados das atividades do utilizador.
   * 
   * @param metricsService Serviço que faz chamadas à API para buscar métricas e dados estatísticos
   */
  constructor(private metricsService: MetricsService) { }

  /**
   * Ciclo de vida do componente - inicializa carregando todas as métricas.
   */
  ngOnInit(): void {
    this.loadAllMetrics();
  }

  /**
   * Carrega todos os dados de métricas relevantes para os gráficos:
   * - Contagem de atividades por tipo
   * - Evolução temporal das atividades
   * - Distribuição por tipo de planta
   */
  loadAllMetrics(): void {
    this.isLoading = true;

    this.metricsService.getActivityCounts(this.timeFrame).subscribe(data => {
      this.activityCounts = data;
    });

    this.metricsService.getActivityByDate('Watering', this.timeFrame).subscribe(data => {
      this.wateringData = this.formatChartData(data, 'Watering');
    });

    this.metricsService.getActivityByDate('Harvest', this.timeFrame).subscribe(data => {
      this.harvestData = this.formatChartData(data, 'Harvest');
    });

    this.metricsService.getActivityByDate('Planting', this.timeFrame).subscribe(data => {
      this.plantingData = this.formatChartData(data, 'Planting');
    });

    this.metricsService.getPlantTypeDistribution().subscribe(data => {
      this.plantTypeData = data.map((item: any) => {
        return {
          name: item.plantType,
          value: item.count
        };
      });
      this.isLoading = false;
    });
  }

  /**
   * Converte os dados brutos da API em formato esperado pelo componente de gráfico.
   * @param data Lista de objetos com { date, count }
   * @param name Nome da série (usado como legenda)
   * @returns Array formatada para o gráfico
   */
  formatChartData(data: any[], name: string): any[] {
    const seriesData = data.map(item => {
      return {
        name: item.date,
        value: item.count
      };
    });

    return [{
      name: name,
      series: seriesData
    }];
  }

  onTimeFrameChange(timeFrame: string): void {
    this.timeFrame = timeFrame;
    this.loadAllMetrics();
  }
}
