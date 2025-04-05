import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MetricsService {
  private baseUrl = 'api/metrics';

  constructor(private http: HttpClient) { }

  getActivityCounts(timeFrame: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/activity-counts?timeFrame=${timeFrame}`);
  }

  getActivityByDate(eventType: string, timeFrame: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/activity-by-date?eventType=${eventType}&timeFrame=${timeFrame}`);
  }

  getPlantTypeDistribution(): Observable<any> {
    return this.http.get(`${this.baseUrl}/metrics/plant-type-distribution`);
  }
}
