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
import { FeatureActionService, FeatureActionUpdateModel, FeatureActionViewModel, SelectModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-feature-action-update',
  templateUrl: './feature-action-update.component.html',
  styleUrls: ['./feature-action-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule],
  providers: [FeatureActionService]
})

export class FeatureActionUpdateComponent implements OnInit {

  // Default feature id
  private _featureId: string = "-1";
  
  // Select list
  actions: SelectModel[] = [];

  // Feature update model
  featureActionUpdateModel: FeatureActionUpdateModel = new FeatureActionUpdateModel();

  constructor(private featureActionService: FeatureActionService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // Get feature id by url
    this.getFeatureIdByUrl();
  }

  // Get feature id by url
  private getFeatureIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._featureId = params["recordId"];

      // Get feature by id
      if (this._featureId != undefined || this._featureId != null || this._featureId! != "") {
        this.getFeatureActionById();
      }
    });
  }

  // Get feature action by id
  private getFeatureActionById(): void {
    this.spinnerService.show();
    this.featureActionService.getById(this._featureId!).subscribe((result: FeatureActionViewModel) => {
      this.featureActionUpdateModel = result.updateModel!;

      // Get select list
      this.actions = result.optionsDataSources.ActionSelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("FEature Action cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Feature Action update form validation
  private getFEatureActionUpdateFromValidationResult(): boolean {

    if (!this.featureActionUpdateModel.actionIds == undefined || this.featureActionUpdateModel.actionIds == null 
      || this.featureActionUpdateModel.actionIds.length == 0) {
      this.toastrService.warning('Please, provide al last one action name.', 'Warning.');
      return false;
    }
    
    return true;
  }

  // On click update feature action
  onClickUpdateFeatureAction(): void {

    // Get feature action update form validation result
    let getUpdateFormValidation: boolean = this.getFEatureActionUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.featureActionService.update(this.featureActionUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Feature Action update successful.", "Successful");
          return this.router.navigateByUrl("/app/feature-actions");
        } else {
          this.toastrService.error("Feature Action cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Feature Action cannot update successful.", "Error");
        return;
      })
    }
  }
}