import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityService, CompanyCreateModel, CompanyService, CompanyViewModel, FileParameter, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadFile, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

@Component({
  selector: 'app-company-create',
  templateUrl: './company-create.component.html',
  styleUrls: ['./company-create.component.css'],
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
    NzUploadModule, 
    NzCheckboxModule
  ],
  providers: [CompanyService, CityService]
})

export class CompanyCreateComponent implements OnInit, OnDestroy {

  // Default company id
  private _companyId: string = "-1";

  // Select list
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];
  currencies: SelectModel[] = [];

  // Company create model
  companyCreateModel: CompanyCreateModel = new CompanyCreateModel();

  // For logo upload
  selectedLogoFile: File | null = null;
  logoFileList: NzUploadFile[] = [];
  logoPreviewUrl: string | null = null;

  private readonly maxLogoSizeInMb = 2;

  constructor(
    private companyService: CompanyService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router, 
    private cityService: CityService) { }

  ngOnInit() {
    // Get company by id
    this.getCompanyByIdAsync();  
  }

  ngOnDestroy(): void {
    this.clearLogoPreview();
  }

  // Get company by id
  private getCompanyByIdAsync(): void {
    this.spinnerService.show();
    this.companyService.getById(this._companyId).subscribe((result: CompanyViewModel) => {
      // Get select list
      this.countries = result.optionsDataSources.CountrySelectList;
      this.currencies = result.optionsDataSources.CurrencySelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Country cannot found based on this id! Please, try again.", "Error.");
      return;
    })
  }

  // Company create form validation
  private getCompanyCreateFromValidationResult(): boolean {

    // Company name validation
    if (!this.companyCreateModel.name || this.companyCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide company name.', 'Warning.');
      return false;
    }

    // Company phone number validation
    if (!this.companyCreateModel.phone || this.companyCreateModel.phone.trim() === '') {
      this.toastrService.warning('Please, provide company phone number.', 'Warning.');
      return false;
    }

    // Company country validation
    if (this.companyCreateModel.countryId == undefined || this.companyCreateModel.countryId == null || this.companyCreateModel.countryId <= 0) {
      this.toastrService.warning('Please, provide country', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyCreateModel.cityId == undefined || this.companyCreateModel.cityId == null || this.companyCreateModel.cityId <= 0) {
      this.toastrService.warning('Please, provide city', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyCreateModel.currencyId == undefined || this.companyCreateModel.currencyId == null || this.companyCreateModel.currencyId <= 0) {
      this.toastrService.warning('Please, provide currency', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // on click save company
  onClickSaveCompany(): void {
    const isValid = this.getCompanyCreateFromValidationResult();

    if (!isValid) {
      return;
    }

    this.spinnerService.show();
    let logoFileParameter: FileParameter | null = null;

    if (this.selectedLogoFile) {
      logoFileParameter = {
        data: this.selectedLogoFile,
        fileName: this.selectedLogoFile.name
      };
    }

    // Set default values for boolean fields if they are undefined
    this.companyCreateModel.isSellWithPos = this.companyCreateModel.isSellWithPos ?? false;
    this.companyCreateModel.isProductHaveBrand = this.companyCreateModel.isProductHaveBrand ?? false;
    this.companyCreateModel.isDefaultCompany = this.companyCreateModel.isDefaultCompany ?? false;

    this.companyService.create(
      this.companyCreateModel.name,
      this.companyCreateModel.email,
      this.companyCreateModel.phone,
      this.companyCreateModel.countryId,
      this.companyCreateModel.cityId,
      this.companyCreateModel.currencyId,
      logoFileParameter,
      this.companyCreateModel.taxNo,
      this.companyCreateModel.isSellWithPos,
      this.companyCreateModel.isProductHaveBrand,
      this.companyCreateModel.isDefaultCompany,
      this.companyCreateModel.address
    ).subscribe({
      next: (result: boolean) => {
        this.spinnerService.hide();

        if (result) {
          this.toastrService.success("Company create successful.", "Successful");
          this.router.navigateByUrl("/app/companies");
        } else {
          this.toastrService.error("Company cannot created! Please, try again.", "Error");
        }
      },
      error: (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Company is not created! Please, try again.", "Error.");
      }
    });
  }

  // On change country
  onChangeCountry(countryId: number): void {
    
    // Get cities by country id
    this.getCitiesByCountryId(countryId);
  }

  // Get cities by country id
  private getCitiesByCountryId(countryId: number): void {
    this.spinnerService.show();
    this.cities = [];
    this.cityService.getCityByCountryId(countryId).subscribe((result: SelectModel[]) => {
      this.cities = result;
      this.spinnerService.hide();
      return;
    },
    (errro: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Cities cannot load based on the selected country! Please, try again.", "Error.");
      return;
    })
  }

  handleChange(info: NzUploadChangeParam): void {
    if (!info.fileList || info.fileList.length === 0) {
      this.removeLogo();
    }
  }

  beforeUpload = (file: NzUploadFile): boolean => {
    const originFile = file as unknown as File;

    if (!originFile) {
      return false;
    }

    const allowedTypes = ['image/png', 'image/jpeg', 'image/jpg'];
    const isValidType = allowedTypes.includes(originFile.type);
    const isValidSize = originFile.size / 1024 / 1024 <= this.maxLogoSizeInMb;

    if (!isValidType) {
      this.toastrService.warning('Please select a PNG or JPG image only.', 'Warning');
      return false;
    }

    if (!isValidSize) {
      this.toastrService.warning(`Logo size must be within ${this.maxLogoSizeInMb} MB.`, 'Warning');
      return false;
    }

    this.clearLogoPreview();

    this.selectedLogoFile = originFile;
    this.logoPreviewUrl = URL.createObjectURL(originFile);

    this.logoFileList = [
      {
        uid: `${Date.now()}`,
        name: originFile.name,
        status: 'done',
        size: originFile.size,
        type: originFile.type
      }
    ];

    // this.toastrService.success('Logo selected successfully.', 'Success');
    return false;
  };

  removeLogo(): void {
    this.selectedLogoFile = null;
    this.logoFileList = [];
    this.clearLogoPreview();
  }

  private clearLogoPreview(): void {
    if (this.logoPreviewUrl) {
      URL.revokeObjectURL(this.logoPreviewUrl);
    }
    this.logoPreviewUrl = null;
  }

  get logoFileSizeKb(): string {
    if (!this.selectedLogoFile?.size) {
      return '0.00';
    }
    return (this.selectedLogoFile.size / 1024).toFixed(2);
  }
}