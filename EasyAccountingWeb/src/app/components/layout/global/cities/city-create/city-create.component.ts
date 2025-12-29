import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityCreateModel, CityService, CityViewModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { SelectModel } from '../../../../../shared/models/select-model';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-city-create',
  templateUrl: './city-create.component.html',
  styleUrls: ['./city-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, NzSelectModule, RouterLink],
  providers: [CityService]
})

export class CityCreateComponent implements OnInit {

  // City id
  private cityId: string = "0";

  // Select list
  countries: SelectModel[] = [];

  // City create model
  cityCreateModel: CityCreateModel = new CityCreateModel();

  constructor(private cityService: CityService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    // Get city by id
    this.getCityById();
  }

  // Get city by id
  private getCityById(): void {
    this.spinnerService.show();
    this.cityService.getById(this.cityId).subscribe({next: (response: CityViewModel) => {
      // Get country select list
      this.countries = response.optionsDataSources.CountrySelectList;
      this.spinnerService.hide();
      return;
    },
    error: (err: any) => {
      this.spinnerService.hide();
      this.toastrService.error("An error occurred while fetching country select list.", "Error");
      return;
    }});
  }

  // City create form validation
  private getCityCreateFromValidationResult(): boolean {

    if (!this.cityCreateModel.name || this.cityCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide city name.', 'Warning.');
      return false;
    } else if (this.cityCreateModel.countryId == undefined || this.cityCreateModel.countryId == null || this.cityCreateModel.countryId == 0) {
      this.toastrService.warning('Please, provide country.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // on click save city
  onClickSaveCity(): void {
    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCityCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.cityService.create(this.cityCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("City create successful.", "Successful");
          return this.router.navigateByUrl("/app/cities");
        } else {
          this.toastrService.error("City cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("City is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}