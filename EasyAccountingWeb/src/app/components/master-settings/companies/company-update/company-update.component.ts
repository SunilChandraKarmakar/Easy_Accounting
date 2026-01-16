import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityService, CompanyService, CompanyUpdateModel, CompanyViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

@Component({
  selector: 'app-company-update',
  templateUrl: './company-update.component.html',
  styleUrls: ['./company-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, RouterLink, NzInputNumberModule, NzSelectModule, NzUploadModule, NzCheckboxModule],
  providers: [CompanyService, CityService]
})

export class CompanyUpdateComponent implements OnInit {

  // Company update model
  companyUpdateModel: CompanyUpdateModel = new CompanyUpdateModel();

  // Select list
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];
  currencies: SelectModel[] = [];

  // Get company id
  private _companyId: string | undefined;

  constructor(private companyService: CompanyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute, private cityService: CityService) { }

  ngOnInit() {

    // Get company id by url
    this.getCompanyIdByUrl();
  }

  // Get company id by url
  private getCompanyIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._companyId = params["recordId"];

      // Get company by id
      if (this._companyId != undefined || this._companyId != null || this._companyId! != "") {
        this.getCompanyById();
      }
    });
  }

  // Get company by id
  private getCompanyById(): void {
    this.spinnerService.show();
    this.companyService.getById(this._companyId!).subscribe((result: CompanyViewModel) => {
      this.companyUpdateModel = result.updateModel!;

      // Get select list
      this.countries = result.optionsDataSources.CountrySelectList;
      this.currencies = result.optionsDataSources.CurrencySelectList;

      // Get cities by country
      this.getCitiesByCountryId(this.companyUpdateModel.countryId);

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Company update form validation
  private getCompanyUpdateFromValidationResult(): boolean {

    // Company name validation
    if (!this.companyUpdateModel.name || this.companyUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide company name.', 'Warning.');
      return false;
    }

    // Company phone number validation
    if (!this.companyUpdateModel.phone || this.companyUpdateModel.phone.trim() === '') {
      this.toastrService.warning('Please, provide company phone number.', 'Warning.');
      return false;
    }

    // Company country validation
    if (this.companyUpdateModel.countryId == undefined || this.companyUpdateModel.countryId == null || this.companyUpdateModel.countryId <= 0) {
      this.toastrService.warning('Please, provide country', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyUpdateModel.cityId == undefined || this.companyUpdateModel.cityId == null || this.companyUpdateModel.cityId <= 0) {
      this.toastrService.warning('Please, provide city', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyUpdateModel.currencyId == undefined || this.companyUpdateModel.currencyId == null || this.companyUpdateModel.currencyId <= 0) {
      this.toastrService.warning('Please, provide currency', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // On click update company
  onClickUpdateCompany(): void {

    // Get company update form validation result
    let getUpdateFormValidation: boolean = this.getCompanyUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.companyService.update(this.companyUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Company update successful.", "Successful");
          return this.router.navigateByUrl("/app/companies");
        } else {
          this.toastrService.error("Company cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Company cannot update successful.", "Error");
        return;
      })
    }
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
    if (info.file.status !== 'uploading') {
    }
    if (info.file.status === 'done') {
    } else if (info.file.status === 'error') {
    }
  }

  cancel(): void { }
}