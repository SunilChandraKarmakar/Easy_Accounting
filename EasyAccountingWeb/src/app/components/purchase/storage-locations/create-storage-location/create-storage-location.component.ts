import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { SelectModel, StorageLocationCreateModel, StorageLocationService, StorageLocationViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-storage-location',
  templateUrl: './create-storage-location.component.html',
  styleUrls: ['./create-storage-location.component.css'],
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
    NzSelectModule
  ],
  providers: [StorageLocationService]
})

export class CreateStorageLocationComponent implements OnInit {

  // Default storage location id
  private _storageLocationId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Storage Location create model
  storageLocationCreateModel: StorageLocationCreateModel = new StorageLocationCreateModel();

  constructor(
    private storageLocationService: StorageLocationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.getStorageLocationById(this._storageLocationId);
  }

  // Get storage location by id
  private getStorageLocationById(storageLocationId: string): void {
    this.spinnerService.show();
    this.storageLocationService.getById(storageLocationId).subscribe((result: StorageLocationViewModel) => {
      
      // Get company select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company select list is not load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Storage Location create form validation
  private getStorageLocationCreateFromValidationResult(): boolean {

    if (this.storageLocationCreateModel.name == undefined || this.storageLocationCreateModel.name == null 
      || this.storageLocationCreateModel.name == "") {
      this.toastrService.warning('Please, provide storage location name.', 'Warning.');
      return false;
    } else if(this.storageLocationCreateModel.companyId == undefined || this.storageLocationCreateModel.companyId == null 
      || this.storageLocationCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click save storage location
  onClickSaveStorageLocation(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getStorageLocationCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.storageLocationService.create(this.storageLocationCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Storage Location create successful.", "Successful");
          return this.router.navigateByUrl("/app/storage-locations");
        } else {
          this.toastrService.error("Storage Location cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Storage Location is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}