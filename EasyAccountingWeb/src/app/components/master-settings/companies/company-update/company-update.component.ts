import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadFile, NzUploadModule } from 'ng-zorro-antd/upload';
import { CityService, CompanyService, CompanyUpdateModel, CompanyViewModel, FileParameter, SelectModel } from '../../../../../api/base-api';
import { environment } from '../../../../../environments/environment.prod';

@Component({
  selector: 'app-company-update',
  templateUrl: './company-update.component.html',
  styleUrls: ['./company-update.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    NzButtonModule,
    NgxSpinnerModule,
    NzInputModule,
    NzIconModule,
    NzBreadCrumbModule,
    RouterLink,
    NzInputNumberModule,
    NzSelectModule,
    NzUploadModule,
    NzCheckboxModule
  ],
  providers: [CompanyService, CityService]
})

export class CompanyUpdateComponent implements OnInit, OnDestroy {

  companyUpdateModel: CompanyUpdateModel = new CompanyUpdateModel();

  countries: SelectModel[] = [];
  cities: SelectModel[] = [];
  currencies: SelectModel[] = [];

  private _companyId: string | undefined;

  selectedLogoFile: File | null = null;
  logoFileList: NzUploadFile[] = [];
  logoPreviewUrl: string | null = null;
  existingLogoUrl: string | null = null;

  private readonly maxLogoSizeInMb = 2;

  constructor(
    private companyService: CompanyService,
    private spinnerService: NgxSpinnerService,
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private cityService: CityService
  ) {}

  ngOnInit(): void {
    this.getCompanyIdByUrl();
  }

  ngOnDestroy(): void {
    this.clearLogoPreview();
  }

  private getCompanyIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._companyId = params['recordId'];

      if (this._companyId && this._companyId.trim() !== '') {
        this.getCompanyById();
      }
    });
  }

  private getCompanyById(): void {
    this.spinnerService.show();

    this.companyService.getById(this._companyId!).subscribe({
      next: (result: CompanyViewModel) => {
        this.companyUpdateModel = result.updateModel!;
        this.companyUpdateModel.isRemoveLogo = false;

        this.countries = result.optionsDataSources.CountrySelectList;
        this.currencies = result.optionsDataSources.CurrencySelectList;

        if (this.companyUpdateModel.logo) {
          this.existingLogoUrl = `${environment.coreBaseUrl}${this.companyUpdateModel.logo}`;
        } else {
          this.existingLogoUrl = null;
        }

        this.getCitiesByCountryId(this.companyUpdateModel.countryId!);
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.toastrService.error('Company cannot found! Please, try again.', 'Error');
      }
    });
  }

  private getCompanyUpdateFromValidationResult(): boolean {
    if (!this.companyUpdateModel.name || this.companyUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide company name.', 'Warning');
      return false;
    }

    if (!this.companyUpdateModel.phone || this.companyUpdateModel.phone.trim() === '') {
      this.toastrService.warning('Please, provide company phone number.', 'Warning');
      return false;
    }

    if (!this.companyUpdateModel.countryId || this.companyUpdateModel.countryId <= 0) {
      this.toastrService.warning('Please, provide country.', 'Warning');
      return false;
    }

    if (!this.companyUpdateModel.cityId || this.companyUpdateModel.cityId <= 0) {
      this.toastrService.warning('Please, provide city.', 'Warning');
      return false;
    }

    if (!this.companyUpdateModel.currencyId || this.companyUpdateModel.currencyId <= 0) {
      this.toastrService.warning('Please, provide currency.', 'Warning');
      return false;
    }

    return true;
  }

  onClickUpdateCompany(): void {
    const isValid = this.getCompanyUpdateFromValidationResult();
    if (!isValid) return;

    this.spinnerService.show();

    let logoFileParameter: FileParameter | null = null;
    let logoPath: string | undefined = this.companyUpdateModel.logo;

    if (this.selectedLogoFile) {
      logoFileParameter = {
        data: this.selectedLogoFile,
        fileName: this.selectedLogoFile.name
      };

      logoPath = undefined;
    }

    this.companyService.update(
      this.companyUpdateModel.id,
      this.companyUpdateModel.name,
      this.companyUpdateModel.email,
      this.companyUpdateModel.phone,
      this.companyUpdateModel.countryId,
      this.companyUpdateModel.cityId,
      this.companyUpdateModel.currencyId,
      logoPath,
      logoFileParameter,
      this.companyUpdateModel.isRemoveLogo,
      this.companyUpdateModel.taxNo,
      this.companyUpdateModel.isSellWithPos ?? false,
      this.companyUpdateModel.isProductHaveBrand ?? false,
      this.companyUpdateModel.isDefaultCompany ?? false,
      this.companyUpdateModel.address
    ).subscribe({
      next: (result: boolean) => {
        this.spinnerService.hide();

        if (result) {
          this.toastrService.success('Company update successful.', 'Successful');
          this.router.navigateByUrl('/app/companies');
        } else {
          this.toastrService.error('Company cannot update successful.', 'Error');
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.toastrService.error('Company cannot update successful.', 'Error');
      }
    });
  }

  onChangeCountry(countryId: number): void {
    this.getCitiesByCountryId(countryId);
  }

  private getCitiesByCountryId(countryId: number): void {
    this.spinnerService.show();
    this.cities = [];

    this.cityService.getCityByCountryId(countryId).subscribe({
      next: (result: SelectModel[]) => {
        this.cities = result;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.toastrService.error('Cities cannot load based on the selected country! Please, try again.', 'Error');
      }
    });
  }

  handleChange(info: NzUploadChangeParam): void {
    if (!info.fileList || info.fileList.length === 0) {
      this.removeSelectedLogo();
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
    this.companyUpdateModel.logo = undefined;
    this.companyUpdateModel.logoFile = undefined;
    this.companyUpdateModel.isRemoveLogo = false;
    this.existingLogoUrl = null;

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

    return false;
  };

  removeSelectedLogo(): void {
    this.selectedLogoFile = null;
    this.companyUpdateModel.logoFile = undefined;
    this.logoFileList = [];
    this.clearLogoPreview();

    // restore previous logo if it exists and was not deleted
    if (!this.companyUpdateModel.isRemoveLogo && this.companyUpdateModel.logo) {
      this.existingLogoUrl = `${environment.coreBaseUrl}${this.companyUpdateModel.logo}`;
    }
  }

  removeExistingLogo(): void {
    this.existingLogoUrl = null;
    this.companyUpdateModel.logo = undefined;
    this.companyUpdateModel.isRemoveLogo = true;
    this.removeSelectedLogo();
  }

  private clearLogoPreview(): void {
    if (this.logoPreviewUrl) {
      URL.revokeObjectURL(this.logoPreviewUrl);
    }

    this.logoPreviewUrl = null;
  }

  get currentLogoUrl(): string | null {
    return this.logoPreviewUrl || this.existingLogoUrl;
  }

  get currentLogoName(): string {
    if (this.selectedLogoFile?.name) {
      return this.selectedLogoFile.name;
    }

    if (this.companyUpdateModel.logo) {
      return this.companyUpdateModel.logo.split('/').pop() ?? 'Current Logo';
    }

    return 'No file selected';
  }

  get logoFileSizeKb(): string {
    if (this.selectedLogoFile?.size) {
      return (this.selectedLogoFile.size / 1024).toFixed(2);
    }

    return '0.00';
  }
}