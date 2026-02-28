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
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeGridModel, EmployeeService, FilterPageModel, FilterPageResultModelOfEmployeeGridModel } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrls: ['./employees.component.css'],
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
    NzPopconfirmModule],
  providers: [EmployeeService]
})

export class EmployeesComponent implements OnInit {

  // Table property
  employees: EmployeeGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private employeeService: EmployeeService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get employees
    this.getEmployees();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get employees
  private getEmployees(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.employees = [];
    this.totalRecord = 0;

    this.employeeService.getFilterEmployees(this.filterPageModel).subscribe((result: FilterPageResultModelOfEmployeeGridModel) => {
      this.employees = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.employees = [];
      this.totalRecord = 0;

      this.toastrService.error("Employee list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get employees
    this.getEmployees();
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

    // Get employees
    this.getEmployees();
  }

  // On click open delete employee
  onClickDelete(employeeId: string | undefined): void {
    this.deleteEmployee(employeeId);
  }

  // Delete employee
  private deleteEmployee(employeeId: string | undefined): void {
    if(employeeId == null || employeeId == undefined) {
      this.toastrService.error("Employee is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.employeeService.delete(employeeId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Employee deleted successfully.", "Success"); 
        this.getEmployees();
      } else {
        this.toastrService.error("Employee is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Employee is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}