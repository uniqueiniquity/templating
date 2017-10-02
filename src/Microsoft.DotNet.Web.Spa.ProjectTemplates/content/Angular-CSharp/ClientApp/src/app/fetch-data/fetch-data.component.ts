import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[];

  constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
    http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
      this.forecasts = result.json() as WeatherForecast[];
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
