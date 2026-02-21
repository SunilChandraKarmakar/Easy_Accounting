import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFeatureActionCreateModel, EmployeeFeatureActionService, EmployeeFeatureActionViewModel, EmployeeService, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

@Component({
  selector: 'app-create-employee-feature-action',
  templateUrl: './create-employee-feature-action.component.html',
  styleUrls: ['./create-employee-feature-action.component.css'],
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
    NzCheckboxModule
  ],
  providers: [EmployeeFeatureActionService, EmployeeService]
})

export class CreateEmployeeFeatureActionComponent implements OnInit {

  // Default employee id
  private _employeeId: string = "-1";

  // Create model
  employeeFeatureActionCreateModel: EmployeeFeatureActionCreateModel = new EmployeeFeatureActionCreateModel(); 
  employeeFeatureActionCreateModels: EmployeeFeatureActionCreateModel[] = []; 

  // Select list
  companies: SelectModel[] = [];
  employees: SelectModel[] = [];
  modules: SelectModel[] = [];
  features: SelectModel[] = [];
  actions: SelectModel[] = [];

  // Store selected permissions
  selectedPermissions: { moduleId: number; featureId: number; actionId: number }[] = [];

  constructor(
    private employeeFeatureActionService: EmployeeFeatureActionService, 
    private employeeService: EmployeeService,
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    
    // Get employee feature action by employee id
    this.getEmployeeFeatureActionByEmployeeId(this._employeeId);
  }

  // Get employee feature action by employee id
  private getEmployeeFeatureActionByEmployeeId(employeeId: string): void {
    this.spinnerService.show();
    this.employeeFeatureActionService.getById(employeeId).subscribe((result: EmployeeFeatureActionViewModel) => {

      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.modules = result.optionsDataSources.ModuleSelectList;
      this.features = result.optionsDataSources.FeatureSelectList;
      this.actions = result.optionsDataSources.ActionSelectList;

      // Group features based on module
      this.groupFeatures();

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Dropdown select list cannot load at this time! Please, try again.", "Error");
      return;
    })
  }

  // On change company
  onChangeCompany(companyId: number): void {
    if(companyId != undefined && companyId != null && companyId > 0) {
      this.getEmployeeByCompanyId(companyId);
    }
  }

  // Get employee based on the company id
  private getEmployeeByCompanyId(companyId: number): void {
    this.spinnerService.hide();
    this.employeeService.getEmployeeByCompanyId(companyId).subscribe((result: SelectModel[]) => {
      this.employees = result;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Employee cannot load based on this company! Please, try again.", "Error.");
      return;
    })
  }

  // Group by feature based on module
  groupedFeatures: Partial<Record<number, SelectModel[]>> = {};
  private groupFeatures(): void {
    this.groupedFeatures = this.features.reduce((acc, feature) => {
      (acc[feature.valueOne] ??= []).push(feature);
      return acc;
    }, {} as Partial<Record<number, SelectModel[]>>);
  }

  onActionChange(moduleId: number, featureId: number, actionId: number, checked: boolean): void {
    if (checked) {
      this.selectedPermissions.push({
        moduleId,
        featureId,
        actionId
      });
    } else {
      this.selectedPermissions = this.selectedPermissions
        .filter(x => !(x.moduleId === moduleId && x.featureId === featureId && x.actionId === actionId));
    }

    console.log(this.selectedPermissions);
  }
}