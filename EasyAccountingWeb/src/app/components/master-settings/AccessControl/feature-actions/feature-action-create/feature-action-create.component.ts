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
import { FeatureActionCreateModel, FeatureActionService, FeatureActionViewModel, FeatureService, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-feature-action-create',
  templateUrl: './feature-action-create.component.html',
  styleUrls: ['./feature-action-create.component.css'],
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
    NzSelectModule
  ],
  providers: [FeatureActionService, FeatureService]
})

export class FeatureActionCreateComponent implements OnInit {

  // Default feature id
  private _featureId: string = "-1";

  // Select list
  modules: SelectModel[] = [];
  actions: SelectModel[] = [];
  features: SelectModel[] = [];

  // Feature create model
  featureActionCreateModel: FeatureActionCreateModel = new FeatureActionCreateModel();

  constructor(private featureActionService: FeatureActionService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService, private router: Router, private featureService: FeatureService) { }

  ngOnInit() {

    // Get feature action by id
    this.getFeatureActionByIdAsync();  
  }

  // Get feature action by id
  private getFeatureActionByIdAsync(): void {
    this.spinnerService.show();
    this.featureActionService.getById(this._featureId).subscribe((result: FeatureActionViewModel) => {
      
      // Get select list
      this.modules = result.optionsDataSources.ModuleSelectList;
      this.actions = result.optionsDataSources.ActionSelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Module & action cannot found! Please, try again.", "Error.");
      return;
    })
  }

  // On change module
  onChangeModule(moduleId: number): void {
    this.spinnerService.show()
    this.featureService.getFeatureByModuleId(moduleId).subscribe((result: SelectModel[]) => {
      this.features = result;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Feature list is not load based on selected module! Please, try again.", "Error");
      return;
    })
  }

  // Create from validation
  private createFromValidation(): boolean {
    if(this.featureActionCreateModel.moduleId == undefined || this.featureActionCreateModel.moduleId == null 
      || this.featureActionCreateModel.moduleId == -1) {
      this.toastrService.warning("Please, select module.", "Warning");
      return false;
    } else if(this.featureActionCreateModel.featureId == undefined || this.featureActionCreateModel.featureId == null 
      || this.featureActionCreateModel.featureId == -1) {
      this.toastrService.warning("Please, select feature.", "Warning.");
      return false;
    } else if(this.featureActionCreateModel.actionIds == undefined || this.featureActionCreateModel.actionIds == null 
      || this.featureActionCreateModel.actionIds.length == 0) {
      this.toastrService.warning("Please, select at last one action for this feature.", "Warning.");
      return false;
    } else {
      return true;
    }
  }

  // Save feature action
  onClickSaveFeatureAction(): void {
    let getCreateFromValidationResult: boolean = this.createFromValidation();

    if(getCreateFromValidationResult) {
      this.spinnerService.show();
      this.featureActionService.create(this.featureActionCreateModel).subscribe((reatlt: boolean) => {
        this.spinnerService.hide();
        this.toastrService.success("Feature action mapping has been set.", "Successful");
        return this.router.navigateByUrl("/app/feature-actions");
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Feature action mapping cannot set! Please, try again");
        return;
      })
    }
  }
}