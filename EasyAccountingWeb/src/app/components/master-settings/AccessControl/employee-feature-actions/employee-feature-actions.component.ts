import { CommonModule } from '@angular/common';
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
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFeatureActionGridModel, EmployeeFeatureActionService, FilterPageModel, FilterPageResultModelOfEmployeeFeatureActionGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employee-feature-actions',
  templateUrl: './employee-feature-actions.component.html',
  styleUrls: ['./employee-feature-actions.component.css'],
  standalone: true,
  imports: [
    CommonModule, 
    NzButtonModule, 
    NzDividerModule, 
    NzTableModule, 
    RouterLink, 
    NgxSpinnerModule, 
    NzSpaceModule, 
    NzInputModule, 
    NzIconModule, 
    NzBreadCrumbModule, 
    NzPopconfirmModule, 
    NzTagModule
  ],
  providers: [EmployeeFeatureActionService]
})

export class EmployeeFeatureActionsComponent implements OnInit {

  // Table property
  employeeFeatureActions: EmployeeFeatureActionGridModel[] = [];
  totalRecord: number = 0;
  
  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private employeeFeatureActionService: EmployeeFeatureActionService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get employee feature actions
    this.getEmployeeFeatureActions();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get employee feature actions
    this.getEmployeeFeatureActions();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "employeename";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
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

    // Get employee feature actions
    this.getEmployeeFeatureActions();
  }

  // Get employee feature actions
  private getEmployeeFeatureActions(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.employeeFeatureActions = [];
    this.totalRecord = 0;

    this.employeeFeatureActionService.getFilterEmployeeFeatureActions(this.filterPageModel)
    .subscribe((result: FilterPageResultModelOfEmployeeFeatureActionGridModel) => {
      this.employeeFeatureActions = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.employeeFeatureActions = [];
      this.totalRecord = 0;

      this.toastrService.error("Employee Feature Action list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  // On click open delete modal
  onClickDelete(featuresId: number): void {
    this.deleteFeatureAction(featuresId);
  }

  // Delete employee feature action
  private deleteFeatureAction(employeeId: number): void {
    if(employeeId == null || employeeId == undefined || employeeId == -1) {
      this.toastrService.error("Employee Feature Action is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.employeeFeatureActionService.delete(employeeId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Employee Feature Action deleted successfully.", "Success"); 
        this.getEmployeeFeatureActions();
      } else {
        this.toastrService.error("Employee Feature Action is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Employee Feature Action is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}