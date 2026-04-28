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
import { CategoryService, CategoryUpdateModel, CategoryViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-update-category',
  templateUrl: './update-category.component.html',
  styleUrls: ['./update-category.component.css'],
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
  providers: [CategoryService]
})

export class UpdateCategoryComponent implements OnInit {

  // Default category id
  private _categoryId: string = "-1";

  // Select list
  companies: SelectModel[] = [];
  parentCategories: SelectModel[] = [];

  // Category update model
  categoryUpdateModel: CategoryUpdateModel = new CategoryUpdateModel();

  constructor(
    private categoryService: CategoryService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private accessControlService: AccessControlService) { }

  ngOnInit() {
    // Set login user permission
    this.accessControlService.setPermissions();

    this.getCategoryIdByUrl();
  }

  // Get category id by url
  private getCategoryIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._categoryId = params["recordId"];

      if (this._categoryId != undefined || this._categoryId != null || this._categoryId! != "") {
        this.getCategoryById(this._categoryId);
      }
    });
  }

  // Get category by id
  private getCategoryById(categoryId: string): void {
    this.spinnerService.show();
    this.categoryService.getById(categoryId).subscribe((result: CategoryViewModel) => {
      
      this.categoryUpdateModel = result.updateModel!;

      // Get company select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.parentCategories = result.optionsDataSources.ParentCategorySelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company select list is not load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Category update form validation
  private getCategoryUpdateFromValidationResult(): boolean {

    if (this.categoryUpdateModel.name == undefined || this.categoryUpdateModel.name == null || this.categoryUpdateModel.name == "") {
      this.toastrService.warning('Please, provide category name.', 'Warning.');
      return false;
    } else if(this.categoryUpdateModel.companyId == undefined || this.categoryUpdateModel.companyId == null 
      || this.categoryUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click update category
  onClickUpdateCategory(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getCategoryUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.categoryService.update(this.categoryUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Category update successful.", "Successful");
          return this.router.navigateByUrl("/app/categories");
        } else {
          this.toastrService.error("Category cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Category is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }
}