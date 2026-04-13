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
import { PurchaseCreateModel, PurchaseService, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';

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
    NzSelectModule
  ],
  providers: [PurchaseService]
})

export class PurchaseItemCreateComponent implements OnInit {

  // Default purchase id
  private purchaseId: string = "-1";

  // Purchase create model
  purchaseCreateModel: PurchaseCreateModel = new PurchaseCreateModel();

  // Select list
  companies: SelectModel[] = [];

  constructor(
    private purchaseService: PurchaseService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {
  }

  // Get purchase by id
  private getPurchaseById(purchaseId: string): void {
    this.spinnerService.show();
    this.purchaseService.getById(this.purchaseId).subscribe((result) => {
      
      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Dropdown list cannot found! Please, try again.", "Error.");
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