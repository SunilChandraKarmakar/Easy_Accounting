import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CompanyGridModel, CompanyService, FilterPageModel, FilterPageResultModelOfCompanyGridModel } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-companies',
  templateUrl: './companies.component.html',
  styleUrls: ['./companies.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule, NzTagModule],
  providers: [CompanyService]
})

export class CompaniesComponent implements OnInit {

  // Table property
  companies: CompanyGridModel[] = [];
  totalRecord: number = 0;
  
  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();
  
  constructor(private companyService: CompanyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get companies
    this.getCompanies();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get companies
    this.getCompanies();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get companies
  private getCompanies(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.companies = [];
    this.totalRecord = 0;

    this.companyService.getFilterCompanies(this.filterPageModel).subscribe((result: FilterPageResultModelOfCompanyGridModel) => {
      this.companies = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.companies = [];
      this.totalRecord = 0;

      this.toastrService.error("Company list is not show at this time! Please, try again.", "Error");
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

    // Get companies
    this.getCompanies();
  }

  // On click open delete modal
  onClickDelete(companyId: string | undefined): void {
    this.deleteCompany(companyId);
  }

  // Delete company
  private deleteCompany(companyId: string | undefined): void {
    if(companyId == null || companyId == undefined) {
      this.toastrService.error("Company is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.companyService.delete(companyId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Company deleted successfully.", "Success"); 
        this.getCompanies();
      } else {
        this.toastrService.error("Company is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}