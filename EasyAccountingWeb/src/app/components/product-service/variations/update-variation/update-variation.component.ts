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
import { SelectModel, VariationService, VariationUpdateModel, VariationViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-variation',
  templateUrl: './update-variation.component.html',
  styleUrls: ['./update-variation.component.css'],
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
  providers: [VariationService]
})

export class UpdateVariationComponent implements OnInit {

  // Default variation id
  private _variationId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Variation update model
  variationUpdateModel: VariationUpdateModel = new VariationUpdateModel();

  constructor(
    private variationService: VariationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.getVariationIdByUrl();
  }

  // Get variation id by url
  private getVariationIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._variationId = params["recordId"];

      if (this._variationId != undefined || this._variationId != null || this._variationId! != "") {
        this.getVariationById(this._variationId);
      }
    });
  }

  // Get variation by id
  private getVariationById(categoryId: string): void {
    this.spinnerService.show();
    this.variationService.getById(categoryId).subscribe((result: VariationViewModel) => {
      
      this.variationUpdateModel = result.updateModel!;

      // Get company select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company select list is not load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Variation update form validation
  private getVariationUpdateFromValidationResult(): boolean {

    if (this.variationUpdateModel.name == undefined || this.variationUpdateModel.name == null || this.variationUpdateModel.name == "") {
      this.toastrService.warning('Please, provide variation name.', 'Warning.');
      return false;
    } else if(this.variationUpdateModel.values == undefined || this.variationUpdateModel.values == null 
      || this.variationUpdateModel.values.length <= 0) {
        this.toastrService.warning("Please, provide value.", "Warning");
        return false;
    } else if(this.variationUpdateModel.companyId == undefined || this.variationUpdateModel.companyId == null 
      || this.variationUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click update variation
  onClickUpdateVariation(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getVariationUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.variationService.update(this.variationUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Variation update successful.", "Successful");
          return this.router.navigateByUrl("/app/variations");
        } else {
          this.toastrService.error("Variation cannot update! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Variation is not update! Please, try again.", "Error.");
        return;
      })
    }
  }
}