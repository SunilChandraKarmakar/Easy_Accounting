import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityCreateModel, CountryCreateModel, CountryService } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@Component({
  selector: 'app-country-create',
  templateUrl: './country-create.component.html',
  styleUrls: ['./country-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzUploadModule,
    NzTableModule, NzBreadCrumbModule, NzDividerModule],
  providers: [CountryService]
})

export class CountryCreateComponent implements OnInit {

  // Country create model
  countryCreateModel: CountryCreateModel = new CountryCreateModel();

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    // Initialize 0 index city for create
    this.initializeCities();
  }

  // Initialize 0 index city for create
  private initializeCities(): void {
    this.countryCreateModel.cities = [];
    
    // Initialize empty city
    let emptyCity = new CityCreateModel();
    emptyCity.name = "";

    this.countryCreateModel.cities.push(emptyCity);
  }

  handleChange(info: NzUploadChangeParam): void {
    if (info.file.status !== 'uploading') {
    }
    if (info.file.status === 'done') {
    } else if (info.file.status === 'error') {
    }
  }

  // Add new empty city row
  addCity(): void {
    const city = new CityCreateModel();
    city.name = '';

    this.countryCreateModel.cities = [
      ...(this.countryCreateModel.cities ?? []),
      city
    ];
  }

  // Remove city by index
  removeCity(index: number): void {
    this.countryCreateModel.cities =
      this.countryCreateModel.cities?.filter((_, i) => i !== index) ?? [];
  }

  // Country create form validation
  private getCountryCreateFromValidationResult(): boolean {

    // Country name validation
    if (!this.countryCreateModel.name || this.countryCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide country name.', 'Warning.');
      return false;
    }

    // Country code validation
    if (!this.countryCreateModel.code || this.countryCreateModel.code.trim() === '') {
      this.toastrService.warning('Please, provide country code.', 'Warning.');
      return false;
    }

    // Cities validation (only if cities exist)
    if (this.countryCreateModel.cities && this.countryCreateModel.cities.length > 0) {

      for (let i = 0; i < this.countryCreateModel.cities.length; i++) {
        const city = this.countryCreateModel.cities[i];

        if (!city.name || city.name.trim() === '') {
          this.toastrService.warning(`Please, provide city name for row ${i + 1}.`, 'Warning.');
          return false;
        }
      }
    }

    // All validations passed
    return true;
  }


  // on click save country with city
  onClickSaveCountry(): void {
    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCountryCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.countryService.create(this.countryCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Country create successful.", "Successful");
          return this.router.navigateByUrl("/app/countries");
        } else {
          this.toastrService.error("Country cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Country is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}