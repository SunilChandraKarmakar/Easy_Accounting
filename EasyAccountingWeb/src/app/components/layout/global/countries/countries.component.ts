import { Component, OnInit } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzTableModule } from 'ng-zorro-antd/table';
import { RouterLink } from "@angular/router";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { CountryGridModel, CountryService, FilterPageModel, FilterPageResultModelOfCountryGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css'],
  standalone: true,
  imports: [NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule],
  providers: [CountryService]
})

export class CountriesComponent implements OnInit {

  // Filter page result model
  countries: CountryGridModel[] = [];
  filterPageModel: FilterPageModel = new FilterPageModel();
  totalRecord: number = 0;

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {    
    // Initialize filter page model
    this.initializeFilterPageModel();

    this.getCountries();
  }

  // Get countries
  private getCountries(): void {
    this.spinnerService.show();
    this.countryService.getFilterCountries(this.filterPageModel).subscribe((result: FilterPageResultModelOfCountryGridModel) => {
      this.countries = result.items || [];
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Country cannot load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Initialize filter page model
  private initializeFilterPageModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "asc";
    this.filterPageModel.filterValue = "";
  }
}