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
import { SelectModel, VariationCreateModel, VariationService, VariationViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-variation',
  templateUrl: './create-variation.component.html',
  styleUrls: ['./create-variation.component.css'],
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

export class CreateVariationComponent implements OnInit {

  // Default variation id
  private _variationId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Variation create model
  variationCreateModel: VariationCreateModel = new VariationCreateModel();

 constructor(
    private variationService: VariationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.getVariationById(this._variationId);
  }

  // Get variation by id
  private getVariationById(categoryId: string): void {
    this.spinnerService.show();
    this.variationService.getById(categoryId).subscribe((result: VariationViewModel) => {
      
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

  // Variation create form validation
  private getVariationCreateFromValidationResult(): boolean {

    if (this.variationCreateModel.name == undefined || this.variationCreateModel.name == null || this.variationCreateModel.name == "") {
      this.toastrService.warning('Please, provide variation name.', 'Warning.');
      return false;
    } else if(this.variationCreateModel.values == undefined || this.variationCreateModel.values == null 
      || this.variationCreateModel.values.length <= 0) {
        this.toastrService.warning("Please, provide value.", "Warning");
        return false;
    } else if(this.variationCreateModel.companyId == undefined || this.variationCreateModel.companyId == null 
      || this.variationCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click save variation
  onClickSaveVariation(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getVariationCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.variationService.create(this.variationCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Variation create successful.", "Successful");
          return this.router.navigateByUrl("/app/variations");
        } else {
          this.toastrService.error("Variation cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Variation is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}