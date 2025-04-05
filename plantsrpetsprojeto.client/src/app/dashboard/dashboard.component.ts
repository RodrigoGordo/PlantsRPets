import { Component, OnInit } from '@angular/core';
import { MetricsService } from '../metrics.service';
import { Color, ScaleType } from '@swimlane/ngx-charts';

/**
 * Componente responsável pela exibição do painel de controlo (dashboard) da aplicação.
 * O dashboard serve para apresentar informações resumidas, métricas, estatísticas e 
 * atalhos para funcionalidades importantes da aplicação de forma visual e interativa.
 */

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  timeFrame: string = 'week';
  activityCounts: any = {};

  // ngx-charts properties
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

  constructor(private metricsService: MetricsService) { }

  ngOnInit(): void {
    this.loadAllMetrics();
  }

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
