import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ProductUnitCreateModel, ProductUnitService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-product-unit',
  templateUrl: './create-product-unit.component.html',
  styleUrls: ['./create-product-unit.component.css'],
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
    NzDividerModule
  ],
  providers: [ProductUnitService]
})

export class CreateProductUnitComponent implements OnInit {

  // Product unit create model
  productUnitCreateModel: ProductUnitCreateModel = new ProductUnitCreateModel();

  constructor(
    private productUnitService: ProductUnitService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
  }

  // Vat production unit form validation
  private getProductionUnitCreateFromValidationResult(): boolean {

    if (this.productUnitCreateModel.name == undefined || this.productUnitCreateModel.name == null 
      || this.productUnitCreateModel.name == "") {
      this.toastrService.warning('Please, provide name.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // on click save Product Unit
  onClickSaveProduct(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getProductionUnitCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.productUnitService.create(this.productUnitCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Production Unit create successful.", "Successful");
          return this.router.navigateByUrl("/app/product-units");
        } else {
          this.toastrService.error("Product Unit cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Product Unit is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}