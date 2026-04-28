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
import { CategoryGridModel, CategoryService, FilterPageModel, FilterPageResultModelOfCategoryGridModel } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css'],
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
  providers: [CategoryService]
})

export class CategoriesComponent implements OnInit {

  // Table property
  categories: CategoryGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private categoryService: CategoryService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private accessControlService: AccessControlService) { }

  ngOnInit() {

    // Set login user permission
    this.accessControlService.setPermissions();

    // Initialize page filter model
    this.initializeFilterModel();

    // Get categories
    this.getCategories();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get categories
  private getCategories(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.categories = [];
    this.totalRecord = 0;

    this.categoryService.getFilterCategories(this.filterPageModel).subscribe((result: FilterPageResultModelOfCategoryGridModel) => {
      this.categories = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.categories = [];
      this.totalRecord = 0;

      this.toastrService.error("Category list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get categories
    this.getCategories();
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

    // Get categories
    this.getCategories();
  }

  // On click open delete category
  onClickDelete(categoryId: string | undefined): void {
    this.deleteCategory(categoryId);
  }

  // Delete category
  private deleteCategory(categoryId: string | undefined): void {
    if(categoryId == null || categoryId == undefined) {
      this.toastrService.error("Category is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.categoryService.delete(categoryId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Category deleted successfully.", "Success"); 
        this.getCategories();
      } else {
        this.toastrService.error("Category is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Category is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}