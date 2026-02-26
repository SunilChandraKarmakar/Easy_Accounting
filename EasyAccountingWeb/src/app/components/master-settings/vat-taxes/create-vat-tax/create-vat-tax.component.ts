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
import { SelectModel, VatTaxCreateModel, VatTaxService, VatTaxViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-create-vat-tax',
  templateUrl: './create-vat-tax.component.html',
  styleUrls: ['./create-vat-tax.component.css'],
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
    NzInputNumberModule
  ],
  providers: [VatTaxService]
})

export class CreateVatTaxComponent implements OnInit {

  // Default vat tax id
  private _vatTaxId: string = "-1";

  // Select list
  companies: SelectModel[]  = [];

  // VatTax create model
  vatTaxCreateModel: VatTaxCreateModel = new VatTaxCreateModel();

  constructor(
    private vatTaxService: VatTaxService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() {

    // Get vat tax by id
    this.getVatTaxById(this._vatTaxId);
  }

  // Get vat tax by id
  private getVatTaxById(vatTaxId: string): void {
    this.spinnerService.show();
    this.vatTaxService.getById(vatTaxId).subscribe((result: VatTaxViewModel) => {
      // Company select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company dropdown list is not load at this time! Please, try again.", "Error.");
      return;
    })
  }

  // Vat tax create form validation
  private getVatTaxCreateFromValidationResult(): boolean {

    if (this.vatTaxCreateModel.taxName == undefined || this.vatTaxCreateModel.taxName == null || this.vatTaxCreateModel.taxName == "") {
      this.toastrService.warning('Please, provide tax name.', 'Warning.');
      return false;
    } else if (this.vatTaxCreateModel.taxNumber == undefined || this.vatTaxCreateModel.taxNumber == null 
      || this.vatTaxCreateModel.taxNumber == "") {
      this.toastrService.warning('Please, provide tax number.', 'Warning.');
      return false;
    } else if (this.vatTaxCreateModel.rate == undefined || this.vatTaxCreateModel.rate == null 
      || this.vatTaxCreateModel.rate < 0) {
      this.toastrService.warning('Please, provide rate.', 'Warning.');
      return false;
    } else if (this.vatTaxCreateModel.companyId == undefined || this.vatTaxCreateModel.companyId == null 
      || this.vatTaxCreateModel.companyId <= 0) {
      this.toastrService.warning('Please, select company.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // on click save vat tax
  onClickSaveVatTax(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getVatTaxCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.vatTaxService.create(this.vatTaxCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Vat Tax create successful.", "Successful");
          return this.router.navigateByUrl("/app/vat-taxes");
        } else {
          this.toastrService.error("Vat Tax cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Vat Tax is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}