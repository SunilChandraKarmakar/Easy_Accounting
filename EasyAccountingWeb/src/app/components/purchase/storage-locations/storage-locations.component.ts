import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FilterPageModel, FilterPageResultModelOfStorageLocationGridModel, StorageLocationGridModel, StorageLocationService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-storage-locations',
  templateUrl: './storage-locations.component.html',
  styleUrls: ['./storage-locations.component.css'],
  standalone: true,
  imports: [
    CommonModule, 
    NzButtonModule, 
    NzDividerModule, 
    NzTableModule, 
    RouterLink, 
    NgxSpinnerModule, 
    NzSpaceModule, 
    NzInputModule, 
    NzIconModule, 
    NzBreadCrumbModule, 
    NzPopconfirmModule,
    CheckPermissionDirective
  ],
  providers: [StorageLocationService]
})

export class StorageLocationsComponent implements OnInit {

  // Table property
  storageLocations: StorageLocationGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private storageLocationService: StorageLocationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private accessControlService: AccessControlService) { }

  ngOnInit() {

    // Set login user permission
    this.accessControlService.setPermissions();

    // Initialize page filter model
    this.initializeFilterModel();

    // Get storage locations
    this.getStorageLocations();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get storage locations
  private getStorageLocations(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.storageLocations = [];
    this.totalRecord = 0;

    this.storageLocationService.getFilterStorageLocations(this.filterPageModel)
    .subscribe((result: FilterPageResultModelOfStorageLocationGridModel) => {
      this.storageLocations = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.storageLocations = [];
      this.totalRecord = 0;

      this.toastrService.error("Storage Location list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get storage locations
    this.getStorageLocations();
  }

  // Table query params
  onChangeQueryParams(event: NzTableQueryParams): void {
    this.filterPageModel.pageIndex = event.pageIndex - 1;
    this.filterPageModel.pageSize = event.pageSize;

    if(event.sort != undefined && event.sort.length > 0) {
      event.sort.forEach(sortObj => {
        if(sortObj.key != null && sortObj.value != null) {
          this.filterPageModel.sortColumn = sortObj.key;
          this.filterPageModel.sortOrder = sortObj.value;
        }
      });
    }

    // Get storage locations
    this.getStorageLocations();
  }

  // On click open delete storage location
  onClickDelete(storageLocationId: string | undefined): void {
    this.deleteStorageLocation(storageLocationId);
  }

  // Delete storage location
  private deleteStorageLocation(storageLocationId: string | undefined): void {
    if(storageLocationId == null || storageLocationId == undefined) {
      this.toastrService.error("Storage Location is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.storageLocationService.delete(storageLocationId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Storage Location deleted successfully.", "Success"); 
        this.getStorageLocations();
      } else {
        this.toastrService.error("Storage Location is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Storage Location is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}