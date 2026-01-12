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
import { CityService, CompanyCreateModel, CompanyService, CompanyViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';

@Component({
  selector: 'app-company-create',
  templateUrl: './company-create.component.html',
  styleUrls: ['./company-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule, NzUploadModule, NzCheckboxModule],
  providers: [CompanyService, CityService]
})

export class CompanyCreateComponent implements OnInit {

  // Default company id
  private _companyId: string = "-1";

  // Select list
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];
  currencies: SelectModel[] = [];

  // Company create model
  companyCreateModel: CompanyCreateModel = new CompanyCreateModel();

  constructor(private companyService: CompanyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private cityService: CityService) { }

  ngOnInit() {
    // Get company by id
    this.getCompanyByIdAsync();  
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

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCompanyCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.companyService.create(this.companyCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Company create successful.", "Successful");
          return this.router.navigateByUrl("/app/companies");
        } else {
          this.toastrService.error("Company cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("COmpany is not created! Please, try again.", "Error.");
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
}