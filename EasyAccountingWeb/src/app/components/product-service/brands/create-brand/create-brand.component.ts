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
import { BrandCreateModel, BrandService, BrandViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-create-brand',
  templateUrl: './create-brand.component.html',
  styleUrls: ['./create-brand.component.css'],
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
  providers: [BrandService]
})

export class CreateBrandComponent implements OnInit {

  // Default brand id
  private _brandId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Brand create model
  brandCreateModel: BrandCreateModel = new BrandCreateModel();

  constructor(
    private brandService: BrandService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.getBrandById(this._brandId);
  }

  // Get brand by id
  private getBrandById(brandId: string): void {
    this.spinnerService.show();
    this.brandService.getById(brandId).subscribe((result: BrandViewModel) => {
      
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

  // Brand create form validation
  private getBrandCreateFromValidationResult(): boolean {

    if (this.brandCreateModel.name == undefined || this.brandCreateModel.name == null || this.brandCreateModel.name == "") {
      this.toastrService.warning('Please, provide brand name.', 'Warning.');
      return false;
    } else if(this.brandCreateModel.companyId == undefined || this.brandCreateModel.companyId == null 
      || this.brandCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click save band
  onClickSaveBrand(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getBrandCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.brandService.create(this.brandCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Brand create successful.", "Successful");
          return this.router.navigateByUrl("/app/brands");
        } else {
          this.toastrService.error("Brand cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Brand is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}