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
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { VendorService, CityService, SelectModel, VendorUpdateModel, VendorViewModel, VendorAddressUpdateModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-vendor',
  templateUrl: './update-vendor.component.html',
  styleUrls: ['./update-vendor.component.css'],
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

export class UpdateVendorComponent implements OnInit {

  // Default vendor id
  private _vendorId: string = "-1";

  // Select list
  companies: SelectModel[] = [];
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];

  // Vendor update model
  vendorUpdateModel: VendorUpdateModel = new VendorUpdateModel();

  constructor(
    private vendorService: VendorService, 
    private cityService: CityService,
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    this.vendorUpdateModel.vendorAddress = new VendorAddressUpdateModel();

    // Get vendor id from route
    this._vendorId = this.activatedRoute.snapshot.paramMap.get('recordId') || "";

    if (this._vendorId) {
      this.getVendorById(this._vendorId);
    } else {
      this.toastrService.error("Vendor id not found.", "Error");
      this.router.navigateByUrl("/app/vendors");
    }
  }

  // Get vendor by id
  private getVendorById(vendorId: string): void {
    this.spinnerService.show();

    this.vendorService.getById(vendorId).subscribe((result: VendorViewModel) => {

      this.vendorUpdateModel = result.updateModel!;

      if (!this.vendorUpdateModel.vendorAddress) {
        this.vendorUpdateModel.vendorAddress = new VendorAddressUpdateModel();
      }

      this.companies = result.optionsDataSources.CompanySelectList;
      this.countries = result.optionsDataSources.CountrySelectList;

      this.getCitiesByCountryId(this.vendorUpdateModel.vendorAddress!.countryId!);
      this.spinnerService.hide();
    });
  }

  // Vendor update form validation
  private getUpdateFromValidationResult(): boolean {

    if (this.vendorUpdateModel.businessName == undefined || this.vendorUpdateModel.businessName == null || 
      this.vendorUpdateModel.businessName == "") {
      this.toastrService.warning('Please, provide business name.', 'Warning.');
      return false;
    } else if(this.vendorUpdateModel.fullName == undefined || this.vendorUpdateModel.fullName == null 
      || this.vendorUpdateModel.fullName == "") {
        this.toastrService.warning("Please, provide full name.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.email == undefined || this.vendorUpdateModel.email == null 
      || this.vendorUpdateModel.email == "") {
        this.toastrService.warning("Please, provide email address.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.phone == undefined || this.vendorUpdateModel.phone == null 
      || this.vendorUpdateModel.phone == "") {
        this.toastrService.warning("Please, provide phone number.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.companyId == undefined || this.vendorUpdateModel.companyId == null 
      || this.vendorUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.vendorAddress?.address == undefined || this.vendorUpdateModel.vendorAddress?.address == null 
      || this.vendorUpdateModel.vendorAddress?.address == "") {
        this.toastrService.warning("Please, provide address.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.vendorAddress?.countryId == undefined || this.vendorUpdateModel.vendorAddress?.countryId == null 
      || this.vendorUpdateModel.vendorAddress?.countryId <= 0) {
        this.toastrService.warning("Please, select country.", "Warning");
        return false;
    } else if(this.vendorUpdateModel.vendorAddress?.cityId == undefined || this.vendorUpdateModel.vendorAddress?.cityId == null 
      || this.vendorUpdateModel.vendorAddress?.cityId <= 0) {
        this.toastrService.warning("Please, select country.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click update vendor
  onClickUpdateVendor(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.vendorService.update(this.vendorUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Vendor update successful.", "Successful");
          return this.router.navigateByUrl("/app/vendors");
        } else {
          this.toastrService.error("Vendor cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Vendor is not updated! Please, try again.", "Error.");
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
    this.getCitiesByCountryId(countryId);
  }

  // Get cities by country id
  private getCitiesByCountryId(countryId: number): void {
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