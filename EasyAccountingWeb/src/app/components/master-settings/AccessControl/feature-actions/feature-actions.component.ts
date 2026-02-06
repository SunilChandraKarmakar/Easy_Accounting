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
import { FeatureActionGridModel, FeatureActionService, FilterPageModel, FilterPageResultModelOfFeatureActionGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzTagModule } from 'ng-zorro-antd/tag';

@Component({
  selector: 'app-feature-actions',
  templateUrl: './feature-actions.component.html',
  styleUrls: ['./feature-actions.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule, NzTagModule],
  providers: [FeatureActionService]
})

export class FeatureActionsComponent implements OnInit {

  // Table property
  featureActions: FeatureActionGridModel[] = [];
  totalRecord: number = 0;
  
  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(private featureActionService: FeatureActionService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get feature actions
    this.getFeatureActions();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get feature actions
    this.getFeatureActions();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
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

    // Get feature actions
    this.getFeatureActions();
  }

  // Get feature actions
  private getFeatureActions(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.featureActions = [];
    this.totalRecord = 0;

    this.featureActionService.getFilterFeatureActions(this.filterPageModel)
    .subscribe((result: FilterPageResultModelOfFeatureActionGridModel) => {
      this.featureActions = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.featureActions = [];
      this.totalRecord = 0;

      this.toastrService.error("Feature Action list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  // On click open delete modal
  onClickDelete(featuresId: number): void {
    this.deleteFeatureAction(featuresId);
  }

  // Delete feature action
  private deleteFeatureAction(featureId: number): void {
    if(featureId == null || featureId == undefined || featureId == -1) {
      this.toastrService.error("Feature is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.featureActionService.delete(featureId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Feature Action deleted successfully.", "Success"); 
        this.getFeatureActions();
      } else {
        this.toastrService.error("Feature Action is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Feature Action is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}