import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { InvoiceSettingCreateModel, InvoiceSettingService, InvoiceSettingViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzColor, NzColorPickerModule } from 'ng-zorro-antd/color-picker';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-Invoice-setting-create',
  templateUrl: './Invoice-setting-create.component.html',
  styleUrls: ['./Invoice-setting-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule, NzUploadModule, NzCheckboxModule, NzColorPickerModule, NzInputNumberModule],
  providers: [InvoiceSettingService]
})

export class InvoiceSettingCreateComponent implements OnInit {

  // Default invoice setting id
  private _invoiceSettingId: string = "-1";

  // Select list
  companies: SelectModel[] = [];

  // Invoice Setting create model
  invoiceSettingCreateModel: InvoiceSettingCreateModel = new InvoiceSettingCreateModel();

  constructor(private invoiceSettingService: InvoiceSettingService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService, private router: Router) { }

  ngOnInit() {

    // Invoice default color
    this.invoiceSettingCreateModel.invoiceColor = "#1677ff";

    // Get invoice setting by id
    this.getInvoiceSettingByIdAsync();  
  }

  // Get invoice setting by id
  private getInvoiceSettingByIdAsync(): void {
    this.spinnerService.show();
    this.invoiceSettingService.getById(this._invoiceSettingId).subscribe((result: InvoiceSettingViewModel) => {
      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;
      this.spinnerService.hide();
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Invoice Setting cannot found based on this id! Please, try again.", "Error.");
      return;
    })
  }

  // Invoice Setting create form validation
  private getInvoiceSettingCreateFromValidationResult(): boolean {

    if (this.invoiceSettingCreateModel.companyId == undefined || this.invoiceSettingCreateModel.companyId == null 
      || this.invoiceSettingCreateModel.companyId <= 0) {
      this.toastrService.warning('Please, provide company', 'Warning.');
      return false;
    }

    if (this.invoiceSettingCreateModel.invoiceDueDateCount == undefined || this.invoiceSettingCreateModel.invoiceDueDateCount == null 
      || this.invoiceSettingCreateModel.invoiceDueDateCount <= 0) {
      this.toastrService.warning('Please, provide invoice due date count', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // on click save Invoice Setting
  onClickSaveInvoiceSetting(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getInvoiceSettingCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.invoiceSettingService.create(this.invoiceSettingCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Invoice Setting create successful.", "Successful");
          return this.router.navigateByUrl("/app/invoice-settings");
        } else {
          this.toastrService.error("Invoice Settings cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Invoice Settings is not created! Please, try again.", "Error.");
        return;
      })
    }
  }

  // On change invoice color
  onInvoiceColorChange(event: { color: NzColor; format: string }): void {
    // Convert NzColor → string
    this.invoiceSettingCreateModel.invoiceColor = event.color.toString();
  }
}