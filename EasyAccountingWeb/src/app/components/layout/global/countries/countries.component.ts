import { Component, OnInit } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { RouterLink } from "@angular/router";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { CountryGridModel, CountryService, FilterPageModel, FilterPageResultModelOfCountryGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzBadgeModule } from 'ng-zorro-antd/badge';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css'],
  standalone: true,
  imports: [NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, NzIconModule, 
    NzTagModule, NzBadgeModule],
  providers: [CountryService]
})

export class CountriesComponent implements OnInit {

  // Table property
  countries: CountryGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get countries
    this.getCountries();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get countries
    this.getCountries();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get countries
  private getCountries(): void {
    this.spinnerService.show();
    this.countryService.getFilterCountries(this.filterPageModel).subscribe((result: FilterPageResultModelOfCountryGridModel) => {
      this.countries = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Country list is not show at this time! Please, try again.", "Error");
      return;
    });
  }
  
  // Table query params
  onChangeQueryParams(event: NzTableQueryParams): void {
    this.filterPageModel.pageIndex = event.pageIndex - 1;
    this.filterPageModel.pageSize = event.pageSize;

    if(event.sort != undefined && event.sort.length > 0) {
      event.sort.forEach(sortObj => {
        if(sortObj.key != null && sortObj.value != null) {
          this.filterPageModel.sortColumn = sortObj.key;
          this.filterPageModel.sortOrder = sortObj.value;
        }
      });
    }

    // Get countries
    this.getCountries();
  }
}