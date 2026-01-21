import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzColor, NzColorPickerModule } from 'ng-zorro-antd/color-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { InvoiceSettingService, InvoiceSettingUpdateModel, InvoiceSettingViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-Invoice-setting-update',
  templateUrl: './Invoice-setting-update.component.html',
  styleUrls: ['./Invoice-setting-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzSelectModule, NzUploadModule, NzCheckboxModule, NzColorPickerModule, NzInputNumberModule],
  providers: [InvoiceSettingService]
})

export class InvoiceSettingUpdateComponent implements OnInit {

  // Invoice Setting update model
  invoiceSettingUpdateModel: InvoiceSettingUpdateModel = new InvoiceSettingUpdateModel();

  // Select list
  companies: SelectModel[] = [];

  // Get invoice setting id
  private _invoiceSettingId: string | undefined;

  constructor(private invoiceSettingService: InvoiceSettingService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // Get invoice setting id by url
    this.getInvoiceSettingIdByUrl();
  }

  // Get invoice setting id by url
  private getInvoiceSettingIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._invoiceSettingId = params["recordId"];

      // Get invoice setting by id
      if (this._invoiceSettingId != undefined || this._invoiceSettingId != null || this._invoiceSettingId! != "") {
        this.getInvoiceSettingById();
      }
    });
  }

  // Get invoice setting by id
  private getInvoiceSettingById(): void {
    this.spinnerService.show();
    this.invoiceSettingService.getById(this._invoiceSettingId!).subscribe((result: InvoiceSettingViewModel) => {
      this.invoiceSettingUpdateModel = result.updateModel!;

      // Get select list
      this.companies = result.optionsDataSources.CompanySelectList;

      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Invoice Setting cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Invoice Setting update form validation
  private getInvoiceSettingUpdateFromValidationResult(): boolean {

    if (this.invoiceSettingUpdateModel.companyId == undefined || this.invoiceSettingUpdateModel.companyId == null 
      || this.invoiceSettingUpdateModel.companyId <= 0) {
      this.toastrService.warning('Please, provide company', 'Warning.');
      return false;
    }

    if (this.invoiceSettingUpdateModel.invoiceDueDateCount == undefined || this.invoiceSettingUpdateModel.invoiceDueDateCount == null 
      || this.invoiceSettingUpdateModel.invoiceDueDateCount <= 0) {
      this.toastrService.warning('Please, provide invoice due date count', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // On click update invoice setting
  onClickUpdateInvoiceSetting(): void {

    // Get invoice setting update form validation result
    let getUpdateFormValidation: boolean = this.getInvoiceSettingUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.invoiceSettingService.update(this.invoiceSettingUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Invoice Setting update successful.", "Successful");
          return this.router.navigateByUrl("/app/invoice-settings");
        } else {
          this.toastrService.error("Invoice Setting cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Invoice Setting cannot update successful.", "Error");
        return;
      })
    }
  }

  cancel(): void { }

  // On change invoice color
  onInvoiceColorChange(event: { color: NzColor; format: string }): void {
    // Convert NzColor → string
    this.invoiceSettingUpdateModel.invoiceColor = event.color.toString();
  }
}