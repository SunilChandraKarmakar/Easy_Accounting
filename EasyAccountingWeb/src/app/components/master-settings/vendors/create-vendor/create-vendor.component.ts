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
import { CityService, SelectModel, VendorAddressCreateModel, VendorCreateModel, VendorService, VendorViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';

@Component({
  selector: 'app-create-vendor',
  templateUrl: './create-vendor.component.html',
  styleUrls: ['./create-vendor.component.css'],
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
    NzUploadModule
  ],
  providers: [VendorService, CityService]
})

export class CreateVendorComponent implements OnInit {

  // Default vendor id
  private _vendorId: string = "-1";

  // Select list
  companies: SelectModel[] = [];
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];

  // Vendor create model
  vendorCreateModel: VendorCreateModel = new VendorCreateModel();

  constructor(
    private vendorService: VendorService, 
    private cityService: CityService,
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {

    // Initialize vendor address create model
    this.vendorCreateModel.vendorAddress = new VendorAddressCreateModel();

    this.getVendorById(this._vendorId);
  }

  // Get vendor by id
  private getVendorById(vendorId: string): void {
    this.spinnerService.show();
    this.vendorService.getById(vendorId).subscribe((result: VendorViewModel) => {
      
      // Set default id for country
      this.vendorCreateModel.vendorAddress!.countryId = -1;

      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.countries = result.optionsDataSources.CountrySelectList;

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Dropdown select list is not load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Vendor create form validation
  private getCreateFromValidationResult(): boolean {

    if (this.vendorCreateModel.businessName == undefined || this.vendorCreateModel.businessName == null || 
      this.vendorCreateModel.businessName == "") {
      this.toastrService.warning('Please, provide business name.', 'Warning.');
      return false;
    } else if(this.vendorCreateModel.fullName == undefined || this.vendorCreateModel.fullName == null 
      || this.vendorCreateModel.fullName == "") {
        this.toastrService.warning("Please, provide full name.", "Warning");
        return false;
    } else if(this.vendorCreateModel.email == undefined || this.vendorCreateModel.email == null 
      || this.vendorCreateModel.email == "") {
        this.toastrService.warning("Please, provide email address.", "Warning");
        return false;
    } else if(this.vendorCreateModel.phone == undefined || this.vendorCreateModel.phone == null 
      || this.vendorCreateModel.phone == "") {
        this.toastrService.warning("Please, provide phone number.", "Warning");
        return false;
    } else if(this.vendorCreateModel.companyId == undefined || this.vendorCreateModel.companyId == null 
      || this.vendorCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else if(this.vendorCreateModel.vendorAddress?.address == undefined || this.vendorCreateModel.vendorAddress?.address == null 
      || this.vendorCreateModel.vendorAddress?.address == "") {
        this.toastrService.warning("Please, provide address.", "Warning");
        return false;
    } else if(this.vendorCreateModel.vendorAddress?.countryId == undefined || this.vendorCreateModel.vendorAddress?.countryId == null 
      || this.vendorCreateModel.vendorAddress?.countryId <= 0) {
        this.toastrService.warning("Please, select country.", "Warning");
        return false;
    } else if(this.vendorCreateModel.vendorAddress?.cityId == undefined || this.vendorCreateModel.vendorAddress?.cityId == null 
      || this.vendorCreateModel.vendorAddress?.cityId <= 0) {
        this.toastrService.warning("Please, select country.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click save vendor
  onClickSaveVendor(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.vendorService.create(this.vendorCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Vendor create successful.", "Successful");
          return this.router.navigateByUrl("/app/vendors");
        } else {
          this.toastrService.error("Vendor cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Vendor is not created! Please, try again.", "Error.");
        return;
      })
    }
  }

  handleChange(info: NzUploadChangeParam): void {
    if (info.file.status !== 'uploading') {
    }
    if (info.file.status === 'done') {
    } else if (info.file.status === 'error') {
    }
  }

  // On change country
  onChangeCountry(countryId: number): void {

    if(countryId != undefined && countryId != null && countryId != -1) {
      this.spinnerService.show();
      this.cityService.getCityByCountryId(countryId).subscribe((result: SelectModel[]) => {
        this.cities = result;
        this.spinnerService.hide();
        return;
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.warning("City dropdown list is not load based on the selected country! Please, try again.", "Error.");
        return;
      })
    }
  }
}