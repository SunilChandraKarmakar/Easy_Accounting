import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { SelectModel, StorageLocationService, StorageLocationUpdateModel, StorageLocationViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-update-storage-location',
  templateUrl: './update-storage-location.component.html',
  styleUrls: ['./update-storage-location.component.css'],
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
    CheckPermissionDirective
  ],
  providers: [StorageLocationService]
})

export class UpdateStorageLocationComponent implements OnInit {

  // Default storage location id
  private _storageLocationId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Storage Location update model
  storageLocationUpdateModel: StorageLocationUpdateModel = new StorageLocationUpdateModel();

  constructor(
    private storageLocationService: StorageLocationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private accessControlService: AccessControlService) { }

  ngOnInit() {

    // Set login user permission
    this.accessControlService.setPermissions();

    // Get storage location id by url
    this.getStorageLocationIdByUrl();
  }

  // Get storage location id by url
  private getStorageLocationIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._storageLocationId = params["recordId"];

      if (this._storageLocationId != undefined || this._storageLocationId != null || this._storageLocationId! != "") {
        this.getStorageLocationById(this._storageLocationId);
      }
    });
  }

  // Get storage location by id
  private getStorageLocationById(storageLocationId: string): void {
    this.spinnerService.show();
    this.storageLocationService.getById(storageLocationId).subscribe((result: StorageLocationViewModel) => {
      
      this.storageLocationUpdateModel = result.updateModel!;

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

  // Storage Location update form validation
  private getStorageLocationUpdateFromValidationResult(): boolean {

    if (this.storageLocationUpdateModel.name == undefined || this.storageLocationUpdateModel.name == null 
      || this.storageLocationUpdateModel.name == "") {
      this.toastrService.warning('Please, provide storage location name.', 'Warning.');
      return false;
    } else if(this.storageLocationUpdateModel.companyId == undefined || this.storageLocationUpdateModel.companyId == null 
      || this.storageLocationUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click update storage location
  onClickUpdateStorageLocation(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getStorageLocationUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.storageLocationService.update(this.storageLocationUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Storage Location update successful.", "Successful");
          return this.router.navigateByUrl("/app/storage-locations");
        } else {
          this.toastrService.error("Storage Location cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Storage Location is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }
}