import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzTableModule } from 'ng-zorro-antd/table';
import { RouterLink } from "@angular/router";
import { NgxSpinnerModule, NgxSpinnerService } from "ngx-spinner";
import { CountryGridModel, CountryService, FilterPageModel, FilterPageResultModelOfCountryGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import {MatPaginator, MatPaginatorModule, PageEvent} from '@angular/material/paginator';
import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatSort, MatSortModule, Sort} from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatCardModule } from '@angular/material/card';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css'],
  standalone: true,
  imports: [NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, NzIconModule, MatFormFieldModule, MatInputModule, MatTableModule, 
    MatSortModule, MatPaginatorModule, MatIconModule, MatDividerModule, MatCardModule],
  providers: [CountryService]
})

export class CountriesComponent implements OnInit, AfterViewInit {

  // Table property
  countries: CountryGridModel[] = [];
  displayedColumns: string[] = ['name', 'code', 'icon'];
  dataSource: MatTableDataSource<CountryGridModel>;
  totalRecord: number = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) {
    
    // Call initial data source
    this.dataSource = new MatTableDataSource(this.countries);
  }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get countries
    this.getCountries();
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event) {
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
    this.filterPageModel.sortOrder = "asc"
    this.filterPageModel.filterValue = "";
  }

  // Get countries
  private getCountries(): void {
    this.spinnerService.show();
    this.countryService.getFilterCountries(this.filterPageModel).subscribe((result: FilterPageResultModelOfCountryGridModel) => {
      this.countries = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.dataSource = new MatTableDataSource(this.countries);
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Country list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  // On page change
  onPageChange(event: PageEvent): void {
    this.filterPageModel.pageIndex = event.pageIndex;
    this.filterPageModel.pageSize = event.pageSize;

    // Get countries
    this.getCountries();
  }

  // On sort change
  onSortChange(event: Sort): void {
    this.filterPageModel.sortColumn = event.active;
    this.filterPageModel.sortOrder = event.direction.toString();
    this.filterPageModel.pageIndex = 0;
    
    // Get countries
    this.getCountries();
  }
  
  onChangePageIndex(event: number): void {
    console.log(" Page index: ", event);
  } 

  onChangePageSize(event: number): void {
    console.log(" Page size: ", event);
  } 

  onChnageQueryParams(event: any): void {
    console.log(" Query params: ", event);
  }
}