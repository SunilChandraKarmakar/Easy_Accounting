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
import { FilterPageModel, FilterPageResultModelOfPurchaseGridModel, PurchaseGridModel, PurchaseService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-purchase-items',
  templateUrl: './purchase-items.component.html',
  styleUrls: ['./purchase-items.component.css'],
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
    NzPopconfirmModule],
  providers: [PurchaseService]
})

export class PurchaseItemsComponent implements OnInit {

  // Table property
  purchases: PurchaseGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();
  
  constructor(
    private purchaseService: PurchaseService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get purchase items
    this.getPurchaseItems();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "vendor";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get purchase items
  private getPurchaseItems(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.purchases = [];
    this.totalRecord = 0;

    this.purchaseService.getFilterPurchases(this.filterPageModel).subscribe((result: FilterPageResultModelOfPurchaseGridModel) => {
      this.purchases = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.purchases = [];
      this.totalRecord = 0;

      this.toastrService.error("Purchase list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get purchase items
    this.getPurchaseItems();
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

    // Get purchase items
    this.getPurchaseItems();
  }

  // On click open delete modal
  onClickDelete(purchaseItemId: string | undefined): void {
    this.deletePurchaseItem(purchaseItemId);
  }

  // Delete purchase item
  private deletePurchaseItem(purchaseItemId: string | undefined): void {
    if(purchaseItemId == null || purchaseItemId == undefined) {
      this.toastrService.error("Purchase item is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.purchaseService.delete(purchaseItemId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Purchase item deleted successfully.", "Success"); 
        this.getPurchaseItems();
      } else {
        this.toastrService.error("Purchase item is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Purchase item is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}