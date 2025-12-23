import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CityCreateModel, CountryCreateModel, CountryService } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NzUploadChangeParam, NzUploadModule } from 'ng-zorro-antd/upload';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';

@Component({
  selector: 'app-country-create',
  templateUrl: './country-create.component.html',
  styleUrls: ['./country-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzUploadModule,
    NzTableModule, NzBreadCrumbModule],
  providers: [CountryService]
})

export class CountryCreateComponent implements OnInit {

  // Country create model
  countryCreateModel: CountryCreateModel = new CountryCreateModel();

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private messageService: NzMessageService) { }

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
      console.log(info.file, info.fileList);
    }
    if (info.file.status === 'done') {
      this.messageService.success(`${info.file.name} file uploaded successfully`);
    } else if (info.file.status === 'error') {
      this.messageService.error(`${info.file.name} file upload failed.`);
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
}