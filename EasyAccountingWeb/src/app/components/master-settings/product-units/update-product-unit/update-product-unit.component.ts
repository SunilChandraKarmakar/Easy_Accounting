import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ProductUnitService, ProductUnitUpdateModel, ProductUnitViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-product-unit',
  templateUrl: './update-product-unit.component.html',
  styleUrls: ['./update-product-unit.component.css'],
  imports: [
    FormsModule, 
    CommonModule, 
    NzButtonModule, 
    RouterLink, 
    NgxSpinnerModule, 
    NzInputModule, 
    NzIconModule, 
    NzBreadCrumbModule, 
    NzDividerModule
  ],
  providers: [ProductUnitService]
})

export class UpdateProductUnitComponent implements OnInit {

  // Default product unit id
  private _productUnitId: string = "-1";

  // Product unit update model
  productUnitUpdateModel: ProductUnitUpdateModel = new ProductUnitUpdateModel();

  constructor(
    private productUnitService: ProductUnitService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    // Get product unit id by url
    this.getProductUnitIdByUrl();
  }

  // Get product unit id by url
  private getProductUnitIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._productUnitId = params["recordId"];

      if (this._productUnitId != undefined || this._productUnitId != null || this._productUnitId! != "") {
        this.getProductUnitById(this._productUnitId);
      }
    });
  }

  // Get product unit by id
  private getProductUnitById(vatTaxId: string): void {
    this.spinnerService.show();
    this.productUnitService.getById(vatTaxId).subscribe((result: ProductUnitViewModel) => {

      this.productUnitUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Production Unit is not load at this time! Please, try again.", "Error.");
      return;
    })
  }

  // Production unit form validation
  private getProductionUnitUpdateFromValidationResult(): boolean {

    if (this.productUnitUpdateModel.name == undefined || this.productUnitUpdateModel.name == null 
      || this.productUnitUpdateModel.name == "") {
      this.toastrService.warning('Please, provide name.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // on click update Product Unit
  onClickUpdateProduct(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getProductionUnitUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.productUnitService.update(this.productUnitUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Production Unit updated successful.", "Successful");
          return this.router.navigateByUrl("/app/product-units");
        } else {
          this.toastrService.error("Product Unit cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Product Unit is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }
}