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
import { FilterPageModel, FilterPageResultModelOfProductUnitGridModel, ProductUnitGridModel, ProductUnitService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-units',
  templateUrl: './product-units.component.html',
  styleUrls: ['./product-units.component.css'],
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
  providers: [ProductUnitService]
})

export class ProductUnitsComponent implements OnInit {

 // Default product unit id
  private _productUnitId: string = "-1";

  // Table property
  productUnits: ProductUnitGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private productUnitService: ProductUnitService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get product units
    this.getProductUnits();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get product units
  private getProductUnits(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.productUnits = [];
    this.totalRecord = 0;

    this.productUnitService.getFilterProductUnits(this.filterPageModel)
    .subscribe((result: FilterPageResultModelOfProductUnitGridModel) => {
      this.productUnits = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.productUnits = [];
      this.totalRecord = 0;

      this.toastrService.error("Product Unit list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get product units
    this.getProductUnits();
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

    // Get product units
    this.getProductUnits();
  }

  // On click open delete product unit
  onClickDelete(productUnitId: string | undefined): void {
    this.deleteProductUnit(productUnitId);
  }

  // Delete vat tax
  private deleteProductUnit(productUnitId: string | undefined): void {
    if(productUnitId == null || productUnitId == undefined) {
      this.toastrService.error("Product Unit is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.productUnitService.delete(productUnitId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Product Unit deleted successfully.", "Success"); 
        this.getProductUnits();
      } else {
        this.toastrService.error("Product Unit is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Product Unit is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}