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
import { BrandService, BrandUpdateModel, BrandViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-brand',
  templateUrl: './update-brand.component.html',
  styleUrls: ['./update-brand.component.css'],
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

export class UpdateBrandComponent implements OnInit {

  // Default brand id
  private _brandId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Brand update model
  brandUpdateModel: BrandUpdateModel = new BrandUpdateModel();

   constructor(
    private brandService: BrandService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    // Get brand id by url
    this.getBrandIdByUrl();
  }

  // Get brand id by url
  private getBrandIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._brandId = params["recordId"];

      if (this._brandId != undefined || this._brandId != null || this._brandId! != "") {
        this.getBrandById(this._brandId);
      }
    });
  }

  // Get brand by id
  private getBrandById(brandId: string): void {
    this.spinnerService.show();
    this.brandService.getById(brandId).subscribe((result: BrandViewModel) => {
      
      this.brandUpdateModel = result.updateModel!;

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

  // Brand update form validation
  private getBrandUpdateFromValidationResult(): boolean {

    if (this.brandUpdateModel.name == undefined || this.brandUpdateModel.name == null || this.brandUpdateModel.name == "") {
      this.toastrService.warning('Please, provide brand name.', 'Warning.');
      return false;
    } else if(this.brandUpdateModel.companyId == undefined || this.brandUpdateModel.companyId == null 
      || this.brandUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click update band
  onClickUpdateBrand(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getBrandUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.brandService.update(this.brandUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Brand update successful.", "Successful");
          return this.router.navigateByUrl("/app/brands");
        } else {
          this.toastrService.error("Brand cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Brand is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }
}