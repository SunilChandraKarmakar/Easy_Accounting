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
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FilterPageModel, FilterPageResultModelOfVatTaxGridModel, SelectModel, VatTaxGridModel, VatTaxService, VatTaxViewModel } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-vat-taxes',
  templateUrl: './vat-taxes.component.html',
  styleUrls: ['./vat-taxes.component.css'],
  standalone: true,
  imports: [
    CommonModule, 
    NzButtonModule, 
    NzDividerModule, 
    NzTableModule, 
    RouterLink, 
    NgxSpinnerModule, 
    NzSpaceModule, 
    NzInputModule, 
    NzIconModule, 
    NzBreadCrumbModule, 
    NzPopconfirmModule],
  providers: [VatTaxService]
})

export class VatTaxesComponent implements OnInit {

  // Default vat tax id
  private _vatTaxIs: string = "-1";

  // Table property
  vatTaxes: VatTaxGridModel[] = [];
  totalRecord: number = 0;

  // Select list
  companies: SelectModel[] = [];

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(
    private vatTaxService: VatTaxService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get vat taxes
    this.getVatTaxes();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get vat taxes
  private getVatTaxes(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.vatTaxes = [];
    this.totalRecord = 0;

    this.vatTaxService.getFilterVatTaxes(this.filterPageModel).subscribe((result: FilterPageResultModelOfVatTaxGridModel) => {
      this.vatTaxes = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.vatTaxes = [];
      this.totalRecord = 0;

      this.toastrService.error("VatTax list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get vat taxes
    this.getVatTaxes();
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

    // Get vat taxes
    this.getVatTaxes();
  }

  // On click open delete modal
  onClickDelete(vatTaxId: string | undefined): void {
    this.deleteVatTax(vatTaxId);
  }

  // Delete vat tax
  private deleteVatTax(vatTAxId: string | undefined): void {
    if(vatTAxId == null || vatTAxId == undefined) {
      this.toastrService.error("Vat Tax is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.vatTaxService.delete(vatTAxId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Vat Tax deleted successfully.", "Success"); 
        this.getVatTaxes();
      } else {
        this.toastrService.error("Vat Tax is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Vat Tax is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}