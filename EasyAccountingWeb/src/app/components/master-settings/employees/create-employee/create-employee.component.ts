import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeCreateModel, EmployeeService, EmployeeViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-employee',
  templateUrl: './create-employee.component.html',
  styleUrls: ['./create-employee.component.css'],
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

export class CreateEmployeeComponent implements OnInit {

  // Default employee id
  private _employeeId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Employee create model
  employeeCreateModel: EmployeeCreateModel = new EmployeeCreateModel();

  constructor(
    private employeeService: EmployeeService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) 
  { }

  ngOnInit() {
    // Get employee by id
    this.getEmployeeByIdAsync();  
  }

  // Get employee by id
  private getEmployeeByIdAsync(): void {
    this.spinnerService.show();
    this.employeeService.getById(this._employeeId).subscribe((result: EmployeeViewModel) => {

      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company dropdown list cannot found! Please, try again.", "Error.");
      return;
    })
  }

  // Employee create form validation
  private getEmployeeCreateFromValidationResult(): boolean {

    if (this.employeeCreateModel.fullName == undefined || this.employeeCreateModel.fullName == null 
      || this.employeeCreateModel.fullName == "") {
      this.toastrService.warning('Please, provide full name.', 'Warning.');
      return false;
    } else if (this.employeeCreateModel.email == undefined || this.employeeCreateModel.email == null 
      || this.employeeCreateModel.email == "") {
      this.toastrService.warning('Please, provide email.', 'Warning.');
      return false;
    } else if (this.employeeCreateModel.password == undefined || this.employeeCreateModel.password == null 
      || this.employeeCreateModel.password == "") {
      this.toastrService.warning('Please, provide password.', 'Warning.');
      return false;
    } else if (this.employeeCreateModel.companyId == undefined || this.employeeCreateModel.companyId == null 
      || this.employeeCreateModel.companyId <= 0) {
      this.toastrService.warning('Please, select company.', 'Warning.');
      return false;
    } else {
      return true;
    } 
  }

  // on click save employee
  onClickSaveEmployee(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getEmployeeCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.employeeService.create(this.employeeCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Employee create successful.", "Successful");
          return this.router.navigateByUrl("/app/employees");
        } else {
          this.toastrService.error("Employee cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Employee is not created! Please, try again.", "Error.");
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