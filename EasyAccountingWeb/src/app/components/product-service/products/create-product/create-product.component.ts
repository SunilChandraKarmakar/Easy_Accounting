import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
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
import { ProductCreateModel, ProductService, ProductViewModel, SelectModel, ProductInventoryCreateModel, CategoryService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { CheckPermissionDirective } from '../../../../identity-shared/directive/check-permission.directive';
import { AccessControlService } from '../../../../identity-shared/services/access-control.service';

@Component({
  selector: 'app-create-product',
  templateUrl: './create-product.component.html',
  styleUrls: ['./create-product.component.css'],
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

export class CreateProductComponent implements OnInit {

  // Default product id
  private _productId: string = "-1";

  // Select list
  productUnits: SelectModel[] = [];
  parentCategories: SelectModel[] = [];
  subCategories: SelectModel[] = [];
  brands: SelectModel[] = [];
  companies: SelectModel[] = [];
  vatTaxes: SelectModel[] = [];

  // Parent Category id
  parentCategoryId: number | undefined;

  // Product create model
  productCreateModel: ProductCreateModel = new ProductCreateModel();

  constructor(
    private productService: ProductService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router,
    private categoryService: CategoryService,
    private accessControlService: AccessControlService) { }

  ngOnInit() {

    // Set login user permission
    this.accessControlService.setPermissions();

    // Initialize product inventory model
    this.productCreateModel.productInventory = new ProductInventoryCreateModel();
    this.productCreateModel.haveProductInventory = false;
    this.productCreateModel.productInventory.haveStockAlert = false;

    this.getProductById(this._productId);
  }

  // Get product by id
  private getProductById(productId: string): void {
    this.spinnerService.show();
    this.productService.getById(productId).subscribe((result: ProductViewModel) => {
      
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

  // Product create form validation
  private getProductCreateFromValidationResult(): boolean {
    if (this.productCreateModel.name == undefined || this.productCreateModel.name == null || this.productCreateModel.name == "") {
      this.toastrService.warning('Please, provide product name.', 'Warning.');
      return false;
    } else if (this.productCreateModel.code == undefined || this.productCreateModel.code == null 
      || this.productCreateModel.code == "") {
      this.toastrService.warning('Please, provide product code.', 'Warning.');
      return false;
    } else if(this.productCreateModel.productUnitId == undefined || this.productCreateModel.productUnitId == null 
      || this.productCreateModel.productUnitId <= 0) {
        this.toastrService.warning("Please, select product unit.", "Warning");
        return false;
    } else if(this.parentCategoryId == undefined || this.parentCategoryId == null || this.parentCategoryId <= 0) {
        this.toastrService.warning("Please, select category.", "Warning");
        return false;
    } else if(this.productCreateModel.categoryId == undefined || this.productCreateModel.categoryId == null 
      || this.productCreateModel.categoryId <= 0) {
        this.toastrService.warning("Please, select sub category.", "Warning");
        return false;
    } else if(this.productCreateModel.brandId == undefined || this.productCreateModel.brandId == null 
      || this.productCreateModel.brandId <= 0) {
        this.toastrService.warning("Please, select brand.", "Warning");
        return false;
    } else if(this.productCreateModel.companyId == undefined || this.productCreateModel.companyId == null 
      || this.productCreateModel.companyId <= 0) {
        this.toastrService.warning("Please, select company.", "Warning");
        return false;
    } else if(this.productCreateModel.costPrice == undefined || this.productCreateModel.costPrice == null 
      || this.productCreateModel.costPrice <= 0) {
        this.toastrService.warning("Please, provide cost price.", "Warning");
        return false;
    } else if(this.productCreateModel.sellPrice == undefined || this.productCreateModel.sellPrice == null 
      || this.productCreateModel.sellPrice <= 0 || this.productCreateModel.sellPrice < this.productCreateModel.costPrice) {
        this.toastrService.warning("Please, provide sell price greater than cost price.", "Warning");
        return false;
    } else if(this.productCreateModel.haveProductInventory 
      && (this.productCreateModel.productInventory?.openingStock == undefined 
      || this.productCreateModel.productInventory?.openingStock == null 
      || this.productCreateModel.productInventory?.openingStock < 0)) {
        this.toastrService.warning("Please, provide stock quantity.", "Warning");
        return false;
    } else if(this.productCreateModel.haveProductInventory 
      && this.productCreateModel.productInventory?.haveStockAlert 
      &&(this.productCreateModel.productInventory?.stockAlertQty == undefined 
      || this.productCreateModel.productInventory?.stockAlertQty == null 
      || this.productCreateModel.productInventory?.stockAlertQty < 0)) {
        this.toastrService.warning("Please, provide stock quantity alert.", "Warning");
        return false;
    }  else {
      return true;
    }
  }

  // on click save product
  onClickSaveProduct(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getProductCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.productService.create(this.productCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Product create successful.", "Successful");
          return this.router.navigateByUrl("/app/products");
        } else {
          this.toastrService.error("Product cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Product is not created! Please, try again.", "Error.");
        return;
      })
    }
  }

  // Handle file upload change
  handleChange(info: any): void {
    if (info.file.status === 'done') {
      this.toastrService.success('Image uploaded successfully', 'Success');
      // Handle the uploaded file URL - you may need to adjust this based on your API response
      this.productCreateModel.image = info.file.response?.url || info.file.name;
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