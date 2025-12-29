import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityService, CityUpdateModel, CityViewModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { SelectModel } from '../../../../../shared/models/select-model';
import { NzSelectModule } from 'ng-zorro-antd/select';

@Component({
  selector: 'app-city-update',
  templateUrl: './city-update.component.html',
  styleUrls: ['./city-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzUploadModule,
    NzTableModule, NzBreadCrumbModule, RouterLink, NzPopconfirmModule, NzSelectModule],
  providers: [CityService]
})

export class CityUpdateComponent implements OnInit {

  listOfOption: Array<{ value: string; label: string }> = [];

  alphabet(size: number): string[] {
  const children: string[] = [];
  for (let i = 10; i < size; i++) {
    children.push(i.toString(36) + i);
  }
  return children;
}



  // City update model
  cityUpdateModel: CityUpdateModel = new CityUpdateModel();
  
  // Get city id
  private _cityId: string | undefined;

  // Select list
  countries: SelectModel[] = [];
  

  constructor(private cityService: CityService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // Get city id by url
    this.getCityIdByUrl();
  }

  // Get city id by url
  private getCityIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._cityId = params["recordId"];

      // Get city by id
      if (this._cityId != undefined || this._cityId != null || this._cityId! != "") {
        this.getCityById();
      }
    });
  }

  // Get city by id
  private getCityById(): void {
    this.spinnerService.show();
    this.cityService.getById(this._cityId!).subscribe((result: CityViewModel) => {
      this.cityUpdateModel = result.updateModel!;
      this.listOfOption = result.optionsDataSources.CountrySelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("City cannot found! Please, try again.", "Error");
      return;
    })
  }

  // City update form validation
  private getCityUpdateFromValidationResult(): boolean {

    if (!this.cityUpdateModel.name || this.cityUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide city name.', 'Warning.');
      return false;
    } else if (this.cityUpdateModel.countryId == undefined || this.cityUpdateModel.countryId == null || this.cityUpdateModel.countryId <= 0) {
      this.toastrService.warning('Please, provide country.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // On click update city
  onClickUpdateCity(): void {

    // Get city update form validation result
    let getUpdateFormValidation: boolean = this.getCityUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.cityService.update(this.cityUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("City update successful.", "Successful");
          return this.router.navigateByUrl("/app/cities");
        } else {
          this.toastrService.error("City cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("City cannot update successful.", "Error");
        return;
      })
    }
  }
}