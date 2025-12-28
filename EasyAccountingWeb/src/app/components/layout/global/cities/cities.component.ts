import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityGridModel, CityService, FilterPageModel, FilterPageResultModelOfCityGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.css'],
  standalone: true,
  imports: [NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, NzIconModule, 
    NzTagModule, NzBadgeModule, NzBreadCrumbModule, NzPopconfirmModule],
  providers: [CityService]
})

export class CitiesComponent implements OnInit {

  // Table property
  cities: CityGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(private cityService: CityService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get cities
    this.getCities();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get cities
    this.getCities();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get cities
  private getCities(): void {
    this.spinnerService.show();
    this.cityService.getFilterCities(this.filterPageModel).subscribe((result: FilterPageResultModelOfCityGridModel) => {
      this.cities = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("City list is not show at this time! Please, try again.", "Error");
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

    console.log("filterPageModel :- ", this.filterPageModel);

    // Get cities 
    this.getCities();
  }

  cancel(): void { }

  // On click open delete modal
  onClickDelete(cityId: string | undefined): void {
    this.deleteCity(cityId);
  }

  // Delete city
  private deleteCity(cityId: string | undefined): void {
    if(cityId == null || cityId == undefined) {
      this.toastrService.error("City is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.cityService.delete(cityId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("City deleted successfully.", "Success"); 
        this.getCities();
      } else {
        this.toastrService.error("City is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("City is not deleted. Please, try again.", "Error");
      return;
    });
  }
}