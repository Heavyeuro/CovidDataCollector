import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public getDeathPlot: string;
  public getCasesPlot: string;
  public getFullTable: string;
  public covidStatModel: CovidStatModel;

  constructor(private readonly http: HttpClient) {
    this.getCasesPlot = "https://localhost:5001/UIData/GetCasesPlot";
    this.getDeathPlot = "https://localhost:5001/UIData/GetDeathPlot";
    this.getFullTable = "https://localhost:5001/UIData/GetStatModel";
    this.http.get<CovidStatModel>(this.getFullTable).subscribe(data => {
      this.covidStatModel = data;
      console.log(this.covidStatModel);
    });
  }
}

export interface CovidStatModel {
  total_cases: number;
  new_cases: number;
  total_deaths: number | null;
  new_deaths: number | null;
}
