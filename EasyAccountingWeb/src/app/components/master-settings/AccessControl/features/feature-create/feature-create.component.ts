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
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FeatureCreateModel, FeatureService, FeatureViewModel, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-feature-create',
  templateUrl: './feature-create.component.html',
  styleUrls: ['./feature-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule],
  providers: [FeatureService]
})

export class FeatureCreateComponent implements OnInit {

  // Default feature id
  private _featureId: string = "-1";

  // Select list
  modules: SelectModel[] = [];

  // Feature create model
  featureCreateModel: FeatureCreateModel = new FeatureCreateModel();

  constructor(private featureService: FeatureService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    // Get feature by id
    this.getFeatureByIdAsync();  
  }

  // Get feature by id
  private getFeatureByIdAsync(): void {
    this.spinnerService.show();
    this.featureService.getById(this._featureId).subscribe((result: FeatureViewModel) => {
      // Get select list
      this.modules = result.optionsDataSources.ModuleSelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Feature cannot found based on this id! Please, try again.", "Error.");
      return;
    })
  }

  // Feature create form validation
  private getFeatureCreateFromValidationResult(): boolean {

    if (!this.featureCreateModel.name || this.featureCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide name.', 'Warning.');
      return false;
    }

    if (!this.featureCreateModel.code || this.featureCreateModel.code.trim() === '') {
      this.toastrService.warning('Please, provide code.', 'Warning.');
      return false;
    }

    if (this.featureCreateModel.moduleId == undefined || this.featureCreateModel.moduleId == null || this.featureCreateModel.moduleId <= 0) {
      this.toastrService.warning('Please, select module', 'Warning.');
      return false;
    }

    if (!this.featureCreateModel.controllerName || this.featureCreateModel.controllerName.trim() === '') {
      this.toastrService.warning('Please, provide controller name.', 'Warning.');
      return false;
    }

    if (!this.featureCreateModel.tableName || this.featureCreateModel.tableName.trim() === '') {
      this.toastrService.warning('Please, provide table name.', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // on click save feature
  onClickSaveFeature(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getFeatureCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.featureService.create(this.featureCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Feature create successful.", "Successful");
          return this.router.navigateByUrl("/app/features");
        } else {
          this.toastrService.error("Feature cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Feature is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}