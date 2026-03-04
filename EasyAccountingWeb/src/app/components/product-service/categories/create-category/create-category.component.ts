import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CategoryCreateModel, CategoryService, CategoryViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-category',
  templateUrl: './create-category.component.html',
  styleUrls: ['./create-category.component.css'],
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
    NzSelectModule
  ],
  providers: [CategoryService]
})

export class CreateCategoryComponent implements OnInit {

  // Default category id
  private _categoryId: string = "-1";

  // Select list
  companies: SelectModel[] = [];
  parentCategories: SelectModel[] = [];

  // Category create model
  categoryCreateModel: CategoryCreateModel = new CategoryCreateModel();

  constructor(
    private categoryService: CategoryService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

   ngOnInit() {
    this.getCategoryById(this._categoryId);
  }

  // Get category by id
  private getCategoryById(categoryId: string): void {
    this.spinnerService.show();
    this.categoryService.getById(categoryId).subscribe((result: CategoryViewModel) => {
      
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

  // Category create form validation
  private getCategoryCreateFromValidationResult(): boolean {

    if (this.categoryCreateModel.name == undefined || this.categoryCreateModel.name == null || this.categoryCreateModel.name == "") {
      this.toastrService.warning('Please, provide category name.', 'Warning.');
      return false;
    } else if(this.categoryCreateModel.companyId == undefined || this.categoryCreateModel.companyId == null 
      || this.categoryCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else {
      return true;
    }
  }

  // on click save category
  onClickSaveCategory(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCategoryCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.categoryService.create(this.categoryCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Category create successful.", "Successful");
          return this.router.navigateByUrl("/app/categories");
        } else {
          this.toastrService.error("Category cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Category is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}