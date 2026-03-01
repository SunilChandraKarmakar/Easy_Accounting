import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeService, EmployeeUpdateModel, EmployeeViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-employee',
  templateUrl: './update-employee.component.html',
  styleUrls: ['./update-employee.component.css'],
  standalone: true,
  imports: [
    FormsModule, 
    CommonModule, 
    NzButtonModule, 
    RouterLink, 
    NgxSpinnerModule, 
    NzInputModule, 
    NzIconModule, 
    NzBreadCrumbModule, 
    NzDividerModule, 
    NzSelectModule, 
    NzUploadModule
  ],
  providers: [EmployeeService]
})

export class UpdateEmployeeComponent implements OnInit {

  // Default employee id
  private _employeeId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Employee update model
  employeeUpdateModel: EmployeeUpdateModel = new EmployeeUpdateModel();

  constructor(
    private employeeService: EmployeeService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute) 
  { }

  ngOnInit() {
    // Get employee id by URL
    this.getEmployeeIdByUrl();  
  }

  // Get employee id by url
  private getEmployeeIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._employeeId = params["recordId"];

      if (this._employeeId != undefined || this._employeeId != null || this._employeeId! != "") {
        this.getEmployeeByIdAsync();
      }
    });
  }

  // Get employee by id
  private getEmployeeByIdAsync(): void {
    this.spinnerService.show();
    this.employeeService.getById(this._employeeId).subscribe((result: EmployeeViewModel) => {

      this.employeeUpdateModel = result.updateModel!;

      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Employee cannot found! Please, try again.", "Error.");
      return;
    })
  }

  // Employee update form validation
  private getEmployeeUpdateFromValidationResult(): boolean {

    if (this.employeeUpdateModel.fullName == undefined || this.employeeUpdateModel.fullName == null 
      || this.employeeUpdateModel.fullName == "") {
      this.toastrService.warning('Please, provide full name.', 'Warning.');
      return false;
    } else if (this.employeeUpdateModel.email == undefined || this.employeeUpdateModel.email == null 
      || this.employeeUpdateModel.email == "") {
      this.toastrService.warning('Please, provide email.', 'Warning.');
      return false;
    } else if (this.employeeUpdateModel.companyId == undefined || this.employeeUpdateModel.companyId == null 
      || this.employeeUpdateModel.companyId <= 0) {
      this.toastrService.warning('Please, select company.', 'Warning.');
      return false;
    } else {
      return true;
    } 
  }

  // on click update employee
  onClickUpdateEmployee(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getEmployeeUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.employeeService.update(this.employeeUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Employee update successful.", "Successful");
          return this.router.navigateByUrl("/app/employees");
        } else {
          this.toastrService.error("Employee cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Employee is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }

  handleChange(info: NzUploadChangeParam): void {
    if (info.file.status !== 'uploading') {
    }
    if (info.file.status === 'done') {
    } else if (info.file.status === 'error') {
    }
  }
}