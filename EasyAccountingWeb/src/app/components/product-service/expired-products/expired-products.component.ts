import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FilterPageModel, FilterPageResultModelOfProductGridModel, ProductGridModel, ProductService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-expired-products',
  templateUrl: './expired-products.component.html',
  styleUrls: ['./expired-products.component.css'],
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
    NzBadgeModule
  ],
  providers: [ProductService]
})

export class ExpiredProductsComponent implements OnInit {

  // Table property
  products: ProductGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private productService: ProductService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get products
    this.getProducts();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get products
  private getProducts(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.products = [];
    this.totalRecord = 0;

    this.productService.getFilterExpiredProducts(this.filterPageModel)
    .subscribe((result: FilterPageResultModelOfProductGridModel) => {
      this.products = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.products = [];
      this.totalRecord = 0;

      this.toastrService.error("Product list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get products
    this.getProducts();
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

    // Get products
    this.getProducts();
  }
}