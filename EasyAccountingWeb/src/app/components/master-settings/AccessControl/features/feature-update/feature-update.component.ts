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
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FeatureService, FeatureUpdateModel, FeatureViewModel, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-feature-update',
  templateUrl: './feature-update.component.html',
  styleUrls: ['./feature-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule],
  providers: [FeatureService]
})

export class FeatureUpdateComponent implements OnInit {
  
  // Feature id
  private _featureId: string = "-1";
  
  // Select list
  modules: SelectModel[] = [];

  // Feature update model
  featureUpdateModel: FeatureUpdateModel = new FeatureUpdateModel();

  constructor(private featureService: FeatureService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

   ngOnInit() {

    // Get feature id by url
    this.getFeatureIdByUrl();
  }

  // Get feature id by url
  private getFeatureIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._featureId = params["recordId"];

      if (this._featureId != undefined || this._featureId != null || this._featureId! != "") {
        this.getFeatureById();
      }
    });
  }

  // Get feature by id
  private getFeatureById(): void {
    this.spinnerService.show();
    this.featureService.getById(this._featureId!).subscribe((result: FeatureViewModel) => {
      this.featureUpdateModel = result.updateModel!;

      // Get select list
      this.modules = result.optionsDataSources.ModuleSelectList;

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Feature cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Feature update form validation
  private getFeatureUpdateFromValidationResult(): boolean {

    if (!this.featureUpdateModel.name || this.featureUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide name.', 'Warning.');
      return false;
    }

    if (!this.featureUpdateModel.code || this.featureUpdateModel.code.trim() === '') {
      this.toastrService.warning('Please, provide code.', 'Warning.');
      return false;
    }

    if (this.featureUpdateModel.moduleId == undefined || this.featureUpdateModel.moduleId == null || this.featureUpdateModel.moduleId <= 0) {
      this.toastrService.warning('Please, select module', 'Warning.');
      return false;
    }

    if (!this.featureUpdateModel.controllerName || this.featureUpdateModel.controllerName.trim() === '') {
      this.toastrService.warning('Please, provide controller name.', 'Warning.');
      return false;
    }

    if (!this.featureUpdateModel.tableName || this.featureUpdateModel.tableName.trim() === '') {
      this.toastrService.warning('Please, provide table name.', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // On click update feature
  onClickUpdateFeature(): void {

    // Get feature update form validation result
    let getUpdateFormValidation: boolean = this.getFeatureUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.featureService.update(this.featureUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Feature update successful.", "Successful");
          return this.router.navigateByUrl("/app/features");
        } else {
          this.toastrService.error("Feature cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Feature cannot update successful.", "Error");
        return;
      })
    }
  }
}