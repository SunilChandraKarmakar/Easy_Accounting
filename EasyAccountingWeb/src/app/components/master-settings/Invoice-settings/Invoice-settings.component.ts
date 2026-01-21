import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzTableModule, NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FilterPageModel, FilterPageResultModelOfInvoiceSettingGridModel, InvoiceSettingGridModel, InvoiceSettingService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzColorPickerModule } from 'ng-zorro-antd/color-picker';
import { presetColors } from 'ng-zorro-antd/core/color';

@Component({
  selector: 'app-Invoice-settings',
  templateUrl: './Invoice-settings.component.html',
  styleUrls: ['./Invoice-settings.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule, NzTagModule, NzColorPickerModule],
  providers: [InvoiceSettingService]
})

export class InvoiceSettingsComponent implements OnInit {

  // Table property
  invoiceSettings: InvoiceSettingGridModel[] = [];
  totalRecord: number = 0;
  
  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();
  
  constructor(private invoiceSettingService: InvoiceSettingService, private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get invoice settings
    this.getInvoiceSettings();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get invoice settings
    this.getInvoiceSettings();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "companyName";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get invoice settings
  private getInvoiceSettings(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.invoiceSettings = [];
    this.totalRecord = 0;

    this.invoiceSettingService.getFilterInvoiceSettings(this.filterPageModel)
      .subscribe((result: FilterPageResultModelOfInvoiceSettingGridModel) => {
      this.invoiceSettings = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.invoiceSettings = [];
      this.totalRecord = 0;

      this.toastrService.error("Invoice Settings list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  // Table query params
  onChangeQueryParams(event: NzTableQueryParams): void {
    this.filterPageModel.pageIndex = event.pageIndex - 1;
    this.filterPageModel.pageSize = event.pageSize;

    if(event.sort != undefined && event.sort.length > 0) {
      event.sort.forEach(sortObj => {
        if(sortObj.key != null && sortObj.value != null) {
          this.filterPageModel.sortColumn = sortObj.key;
          this.filterPageModel.sortOrder = sortObj.value;
        }
      });
    }

    // Get invoice settings
    this.getInvoiceSettings();
  }

  // On click open delete modal
  onClickDelete(invoiceSettingId: string | undefined): void {
    this.deleteInvoiceSetting(invoiceSettingId);
  }

  // Delete invoice setting
  private deleteInvoiceSetting(invoiceSettingId: string | undefined): void {
    if(invoiceSettingId == null || invoiceSettingId == undefined) {
      this.toastrService.error("Invoice Setting is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.invoiceSettingService.delete(invoiceSettingId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Invoice Setting deleted successfully.", "Success"); 
        this.getInvoiceSettings();
      } else {
        this.toastrService.error("Invoice Setting is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Invoice Setting is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}