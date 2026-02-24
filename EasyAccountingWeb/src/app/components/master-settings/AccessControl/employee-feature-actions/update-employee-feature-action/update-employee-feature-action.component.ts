import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFeatureActionService, EmployeeFeatureActionUpdateModel, EmployeeFeatureActionViewModel, EmployeeService, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-employee-feature-action',
  templateUrl: './update-employee-feature-action.component.html',
  styleUrls: ['./update-employee-feature-action.component.css'],
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

export class UpdateEmployeeFeatureActionComponent implements OnInit {

  // Default employee id
  private _getEmployeeIdByUrl: string = "-1";
  private _getSelectedEmployeeId: number | undefined

  // Employee & company id
  employeeName: string | undefined;
  companyName: string | undefined

  // Store selected permissions
  selectedPermissions: { moduleId: number; featureId: number; actionId: number }[] = [];

  // update model
  employeeFeatureActionUpdateModels: EmployeeFeatureActionUpdateModel[] = []; 

  // Select list
  modules: SelectModel[] = [];
  features: SelectModel[] = [];
  actions: SelectModel[] = [];

  constructor(
    private employeeFeatureActionService: EmployeeFeatureActionService, 
    private employeeService: EmployeeService,
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    
    // Get selected employee id by url
    this.getEmployeeIdByUrl();
  }

  // Get selected employee id by url
  private getEmployeeIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._getEmployeeIdByUrl = params["recordId"];

      if (this._getEmployeeIdByUrl != undefined || this._getEmployeeIdByUrl != null || this._getEmployeeIdByUrl! != "") {
        this.getEmployeeFeatureActionByEmployeeId(this._getEmployeeIdByUrl);
      }
    });
  }

  // Get employee feature action by employee id
  private getEmployeeFeatureActionByEmployeeId(employeeId: string): void {
    
    this.spinnerService.show();
    this.employeeFeatureActionService.getById(employeeId).subscribe({next: (result: EmployeeFeatureActionViewModel) => {

      // Update model list
      this.employeeFeatureActionUpdateModels = result.updateModel || [];

      if (this.employeeFeatureActionUpdateModels.length > 0) {
        this.companyName = this.employeeFeatureActionUpdateModels[0].companyName;
        this.employeeName = this.employeeFeatureActionUpdateModels[0].employeeName;
        this._getSelectedEmployeeId = this.employeeFeatureActionUpdateModels[0].employeeId;
      }

      // Load select lists
      this.modules = result.optionsDataSources.ModuleSelectList;
      this.features = result.optionsDataSources.FeatureSelectList;
      this.actions = result.optionsDataSources.ActionSelectList;

      // Group features by module
      this.groupFeatures();

      // Preload selected permissions
      this.selectedPermissions = this.employeeFeatureActionUpdateModels.map(x => ({
        moduleId: x.moduleId!,
        featureId: x.featureId!,
        actionId: x.actionId!
      }));

      this.spinnerService.hide();
    },
    error: () => {
      this.spinnerService.hide();
      this.toastrService.error("Employee Feature Action cannot load at this time! Please, try again.", "Error");
    }});
  }

  // Group by feature based on module
  groupedFeatures: Partial<Record<number, SelectModel[]>> = {};
  private groupFeatures(): void {
    if (!this.features || !this.features.length) {
      this.groupedFeatures = {};
      return;
    }

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
  }

  // Update from validation
  private getUpdateFromValidationResult(): boolean {
    if(this.employeeName == undefined || this.employeeName == null || this.employeeName == "") {
      this.toastrService.warning("Please, provide employee.", "Warning");
      return false;
    } else if(this.selectedPermissions.length == 0 || this.selectedPermissions == undefined || this.selectedPermissions == null) {
        this.toastrService.warning("Please, provide at last one employee permission.", "Warning");
      return false;
    } else {
      return true;
    }
  }

  // On click update employee feature action
  onClickUpdateEmployeeFeatureAction(): void {

    let getValidationResult: boolean = this.getUpdateFromValidationResult();

    if(getValidationResult) {
      this.spinnerService.show();

      this.employeeFeatureActionUpdateModels = this.selectedPermissions.map(x => {
        const model = new EmployeeFeatureActionUpdateModel();
        model.employeeId = this._getSelectedEmployeeId;
        model.featureId = x.featureId;
        model.actionId = x.actionId;
        return model;
      });

      this.employeeFeatureActionService.update(this.employeeFeatureActionUpdateModels).subscribe((result: boolean) => {
        this.spinnerService.hide();
        this.toastrService.success("Feature & action permission is assigned on the selected employee.", "Success.");
        return this.router.navigateByUrl("/app/employee-feature-actions");
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Feature & action permission is not assigned on the selected employee.", "Error");
        return;
      })
    }
  }

  isChecked(moduleId: number, featureId: number, actionId: number): boolean {
    return this.selectedPermissions.some(x =>
      x.moduleId === moduleId &&
      x.featureId === featureId &&
      x.actionId === actionId
    );
  }
}