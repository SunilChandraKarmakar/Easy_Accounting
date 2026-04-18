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
import { PurchaseCreateModel, PurchaseService, SelectModel, VendorService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';

@Component({
  selector: 'app-purchase-item-create',
  templateUrl: './purchase-item-create.component.html',
  styleUrls: ['./purchase-item-create.component.css'],
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
    NzDatePickerModule
  ],
  providers: [PurchaseService, VendorService]
})

export class PurchaseItemCreateComponent implements OnInit {

  date = null;

  // Default purchase id
  private purchaseId: string = "-1";

  // Purchase create model
  purchaseCreateModel: PurchaseCreateModel = new PurchaseCreateModel();

  // Select list
  companies: SelectModel[] = [];
  vendors: SelectModel[] = [];

  constructor(
    private purchaseService: PurchaseService, 
    private vendorService: VendorService,
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.getPurchaseById(this.purchaseId);
  }

  // Get purchase by id
  private getPurchaseById(purchaseId: string): void {
    this.spinnerService.show();
    this.purchaseService.getById(purchaseId).subscribe((result) => {
      
      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company dropdown list cannot found! Please, try again.", "Error.");
      return;
    })
  }

  // On change company
  onChangeCompany(companyId: number): void {
    if(companyId != undefined && companyId != null && companyId > 0) {
      this.getVendors(companyId);
    }
  }

  // Get vendor dropdown list based on the selected company id
  private getVendors(companyId: number): void {
    this.spinnerService.show();
    this.vendorService.getVendorByCompanyId(companyId).subscribe((result: SelectModel[]) => {
      this.vendors = result;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.warning("Vendor dropdown is not load based on the selected company! Please, try again.", "Error.");
      return;
    })
  }

  // Purchase create form validation
  private getPurchaseCreateFromValidationResult(): boolean {

    if (!this.purchaseCreateModel.orderNumber || this.purchaseCreateModel.orderNumber.trim() === '') {
      this.toastrService.warning('Please, provide order number.', 'Warning.');
      return false;
    }

    return true;
  }

  // on click save purchase
  onClickSavePurchase(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getPurchaseCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.purchaseService.create(this.purchaseCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Purchase create successful.", "Successful");
          return this.router.navigateByUrl("/app/purchase-items");
        } else {
          this.toastrService.error("Purchase cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Purchase is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}