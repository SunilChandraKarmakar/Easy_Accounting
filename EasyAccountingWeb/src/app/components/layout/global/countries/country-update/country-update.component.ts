import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CountryService, CountryUpdateModel, CountryViewModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-country-update',
  templateUrl: './country-update.component.html',
  styleUrls: ['./country-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzUploadModule,
    NzTableModule, NzBreadCrumbModule, RouterLink],
  providers: [CountryService]
})

export class CountryUpdateComponent implements OnInit {

  // Country update model
  countryUpdateModel: CountryUpdateModel = new CountryUpdateModel();

  // Get country id
  private _countryId: string | undefined;

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
     private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // Get country id by url
    this.getCountryIdByUrl();
  }

  // Get country id by url
  private getCountryIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._countryId = params["recordId"];

      // Get country by id
      if (this._countryId != undefined || this._countryId != null || this._countryId! != "") {
        this.getCountryById();
      }
    });
  }

  // Get country by id
  private getCountryById(): void {
    this.spinnerService.show();
    this.countryService.getById(this._countryId!).subscribe((result: CountryViewModel) => {
      this.countryUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Country cannot found! Please, try again.", "Error");
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

  // Country update form validation
  private getCountryUpdateFromValidationResult(): boolean {

    // Country name validation
    if (!this.countryUpdateModel.name || this.countryUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide country name.', 'Warning.');
      return false;
    }

    // Country code validation
    if (!this.countryUpdateModel.code || this.countryUpdateModel.code.trim() === '') {
      this.toastrService.warning('Please, provide country code.', 'Warning.');
      return false;
    }

    // Cities validation (only if cities exist)
    if (this.countryUpdateModel.cityUpdateModels && this.countryUpdateModel.cityUpdateModels.length > 0) {

      for (let i = 0; i < this.countryUpdateModel.cityUpdateModels.length; i++) {
        const city = this.countryUpdateModel.cityUpdateModels[i];

        if (!city.name || city.name.trim() === '') {
          this.toastrService.warning(`Please, provide city name for row ${i + 1}.`, 'Warning.');
          return false;
        }
      }
    }

    // All validations passed
    return true;
  }

  // On click update country
  onClickUpdateCountry(): void {

    // Get country update form validation result
    let getUpdateFormValidation: boolean = this.getCountryUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.countryService.update(this.countryUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Country update successful.", "Successful");
          return this.router.navigateByUrl("/app/countries");
        } else {
          this.toastrService.error("Country cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Country cannot update successful.", "Error");
        return;
      })
    }
  }
}