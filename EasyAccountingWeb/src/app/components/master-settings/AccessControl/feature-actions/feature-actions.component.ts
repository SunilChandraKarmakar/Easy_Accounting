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
import { FeatureActionGridModel, FeatureActionService, FilterPageModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-feature-actions',
  templateUrl: './feature-actions.component.html',
  styleUrls: ['./feature-actions.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule],
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
    // this.getFeatures();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get feature actions
    // this.getFeatures();
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
    // this.getFeatures();
  }

  // On click open delete modal
  onClickDelete(featuresId: string | undefined): void {
    // this.deleteFeature(featuresId);
  }

  // Delete feature
  private deleteFeature(featureId: string | undefined): void {
    // if(featureId == null || featureId == undefined) {
    //   this.toastrService.error("Feature is not found. Please, try again.", "Error");
    //   return;
    // }

    // this.spinnerService.show();
    // this.featureService.delete(featureId).subscribe((result: boolean) => {
    //   this.spinnerService.hide();
    //   if(result) {
    //     this.toastrService.success("Feature deleted successfully.", "Success"); 
    //     this.getFeatures();
    //   } else {
    //     this.toastrService.error("Feature is not deleted. Please, try again.", "Error");
    //   }

    //   return;
    // },
    // (error: any) => {
    //   this.spinnerService.hide();
    //   this.toastrService.error("Feature is not deleted. Please, try again.", "Error");
    //   return;
    // });
  }

  cancel(): void { }
}