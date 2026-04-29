import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ProductUpdateModel, ProductService, ProductViewModel, SelectModel, CategoryService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-update-product',
  templateUrl: './update-product.component.html',
  styleUrls: ['./update-product.component.css'],
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
    NzCheckboxModule,
    NzDatePickerModule,
    NzUploadModule,
    CheckPermissionDirective
  ],
  providers: [ProductService, CategoryService]
})

export class UpdateProductComponent implements OnInit {

  // Product id from route
  private _productId: string = "";

  // Select list
  productUnits: SelectModel[] = [];
  parentCategories: SelectModel[] = [];
  subCategories: SelectModel[] = [];
  brands: SelectModel[] = [];
  companies: SelectModel[] = [];
  vatTaxes: SelectModel[] = [];

  // Product update model
  productUpdateModel: ProductUpdateModel = new ProductUpdateModel();

  constructor(
    private productService: ProductService,
    private spinnerService: NgxSpinnerService,
    private toastrService: ToastrService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private categoryService: CategoryService,
    private accessControlService: AccessControlService) { }

  ngOnInit() {

    // Set login user permission
    this.accessControlService.setPermissions();

    // Get product id from route
    this._productId = this.activatedRoute.snapshot.paramMap.get('recordId') || "";

    if (this._productId) {
      this.getProductById(this._productId);
    } else {
      this.toastrService.error("Product ID not found.", "Error");
      this.router.navigateByUrl("/app/products");
    }
  }

  // Get product by id
  private getProductById(productId: string): void {
    this.spinnerService.show();
    this.productService.getById(productId).subscribe((result: ProductViewModel) => {

      // Set update model data
      if (result.updateModel) {
        this.productUpdateModel = result.updateModel;

        // Set parent category if category exists
        if (this.productUpdateModel.categoryId) {
          this.getSubCategoryByParentCategoryById(this.productUpdateModel.parentCategoryId!);
        }
      }

      // Get company select list
      this.productUnits = result.optionsDataSources.ProductUnitSelectList;
      this.parentCategories = result.optionsDataSources.ParentCategorySelectList;
      this.brands = result.optionsDataSources.BrandSelectList;
      this.companies = result.optionsDataSources.CompanySelectList;
      this.vatTaxes = result.optionsDataSources.VatTaxSelectList;

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Dropdown select list is not load at this time! Please, try again.", "Error");
      return;
    })
  }

  // Get parent category by child category id
  private getSubCategoryByParentCategoryById(parentCategoryId: number): void {
    this.getChildCategoryByParentCategoryId(parentCategoryId);
  }

  // Product update form validation
  private getProductUpdateFromValidationResult(): boolean {
    if (this.productUpdateModel.name == undefined || this.productUpdateModel.name == null || this.productUpdateModel.name == "") {
      this.toastrService.warning('Please, provide product name.', 'Warning.');
      return false;
    } else if (this.productUpdateModel.code == undefined || this.productUpdateModel.code == null
      || this.productUpdateModel.code == "") {
      this.toastrService.warning('Please, provide product code.', 'Warning.');
      return false;
    } else if(this.productUpdateModel.productUnitId == undefined || this.productUpdateModel.productUnitId == null
      || this.productUpdateModel.productUnitId <= 0) {
        this.toastrService.warning("Please, select product unit.", "Warning");
        return false;
    } else if(this.productUpdateModel.parentCategoryId == undefined || this.productUpdateModel.parentCategoryId == null 
      || this.productUpdateModel.parentCategoryId <= 0) {
        this.toastrService.warning("Please, select category.", "Warning");
        return false;
    } else if(this.productUpdateModel.categoryId == undefined || this.productUpdateModel.categoryId == null
      || this.productUpdateModel.categoryId <= 0) {
        this.toastrService.warning("Please, select sub category.", "Warning");
        return false;
    } else if(this.productUpdateModel.brandId == undefined || this.productUpdateModel.brandId == null
      || this.productUpdateModel.brandId <= 0) {
        this.toastrService.warning("Please, select brand.", "Warning");
        return false;
    } else if(this.productUpdateModel.companyId == undefined || this.productUpdateModel.companyId == null
      || this.productUpdateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else if(this.productUpdateModel.costPrice == undefined || this.productUpdateModel.costPrice == null
      || this.productUpdateModel.costPrice <= 0) {
        this.toastrService.warning("Please, provide cost price.", "Warning");
        return false;
    } else if(this.productUpdateModel.sellPrice == undefined || this.productUpdateModel.sellPrice == null
      || this.productUpdateModel.sellPrice <= 0 || this.productUpdateModel.sellPrice < this.productUpdateModel.costPrice) {
        this.toastrService.warning("Please, provide sell price greater than cost price.", "Warning");
        return false;
    } else if(this.productUpdateModel.haveProductInventory
      && (this.productUpdateModel.productInventory?.openingStock == undefined
      || this.productUpdateModel.productInventory?.openingStock == null
      || this.productUpdateModel.productInventory?.openingStock <= 0)) {
        this.toastrService.warning("Please, provide stock quantity.", "Warning");
        return false;
    } else if(this.productUpdateModel.haveProductInventory
      && this.productUpdateModel.productInventory?.haveStockAlert
      &&(this.productUpdateModel.productInventory?.stockAlertQty == undefined
      || this.productUpdateModel.productInventory?.stockAlertQty == null
      || this.productUpdateModel.productInventory?.stockAlertQty <= 0)) {
        this.toastrService.warning("Please, provide stock quantity alert.", "Warning");
        return false;
    }  else {
      return true;
    }
  }

  // on click update product
  onClickUpdateProduct(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getProductUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.productService.update(this.productUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();

        if(result) {
          this.toastrService.success("Product update successful.", "Successful");
          return this.router.navigateByUrl("/app/products");
        } else {
          this.toastrService.error("Product cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Product is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }

  // Handle file upload change
  handleChange(info: any): void {
    if (info.file.status === 'done') {
      this.toastrService.success('Image uploaded successfully', 'Success');
      // Handle the uploaded file URL - you may need to adjust this based on your API response
      this.productUpdateModel.image = info.file.response?.url || info.file.name;
    } else if (info.file.status === 'error') {
      this.toastrService.error('Image upload failed', 'Error');
    }
  }

  // On change parent category
  onChangeParentCategory(categoryId: number): void {
    if(categoryId != undefined && categoryId != null && categoryId > 0) {
      this.getChildCategoryByParentCategoryId(categoryId);
    }
  }

  // Get child category based on the parent category id
  private getChildCategoryByParentCategoryId(parentCategoryId: number): void {
    this.spinnerService.show();
    this.categoryService.getCategoriesByParentId(parentCategoryId).subscribe((result: SelectModel[]) => {
      this.subCategories = result;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Child category select list is not load at this time! Please, try again.", "Error");
    });
  }
}