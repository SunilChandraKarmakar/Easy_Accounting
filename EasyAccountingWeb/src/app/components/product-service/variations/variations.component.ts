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
import { FilterPageModel, FilterPageResultModelOfVariationGridModel, VariationGridModel, VariationService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { CheckPermissionDirective } from '../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-variations',
  templateUrl: './variations.component.html',
  styleUrls: ['./variations.component.css'],
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
    NzTagModule,
    CheckPermissionDirective
  ],
  providers: [VariationService]
})

export class VariationsComponent implements OnInit {

  // Table property
  variations: VariationGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private variationService: VariationService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private accessControlService: AccessControlService) { }

 ngOnInit() {
  
    // Set login user permission
    this.accessControlService.setPermissions();

    // Initialize page filter model
    this.initializeFilterModel();

    // Get variations
    this.getVariations();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get variations
  private getVariations(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.variations = [];
    this.totalRecord = 0;

    this.variationService.getFilterVariations(this.filterPageModel).subscribe((result: FilterPageResultModelOfVariationGridModel) => {
      this.variations = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.variations = [];
      this.totalRecord = 0;

      this.toastrService.error("Variation list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get variations
    this.getVariations();
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

    // Get variations
    this.getVariations();
  }

  // On click open delete variation
  onClickDelete(variationId: string | undefined): void {
    this.deleteVariation(variationId);
  }

  // Delete variation
  private deleteVariation(variationId: string | undefined): void {
    if(variationId == null || variationId == undefined) {
      this.toastrService.error("Variation is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.variationService.delete(variationId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Variation deleted successfully.", "Success"); 
        this.getVariations();
      } else {
        this.toastrService.error("Variation is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Variation is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}