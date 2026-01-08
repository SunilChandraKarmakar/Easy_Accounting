import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
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
import { CurrencyGridModel, CurrencyService, FilterPageModel, FilterPageResultModelOfCurrencyGridModel } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.css'],
  standalone: true,
  imports: [NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, NzIconModule, 
    NzTagModule, NzBadgeModule, NzBreadCrumbModule, NzPopconfirmModule],
  providers: [CurrencyService]
})

export class CurrenciesComponent implements OnInit {

  // Table property
  currencies: CurrencyGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();

  constructor(private currencyService: CurrencyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get currencies
    this.getCurrencies();
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get currencies
    this.getCurrencies();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get currencies
  private getCurrencies(): void {
    this.spinnerService.show();
    this.currencyService.getFilterCurrencies(this.filterPageModel).subscribe((result: FilterPageResultModelOfCurrencyGridModel) => {
      this.currencies = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Currency list is not show at this time! Please, try again.", "Error");
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

    // Get currencies
    this.getCurrencies();
  }

  cancel(): void { }

  // On click open delete modal
  onClickDelete(currencyId: string | undefined): void {
    this.deleteCurrency(currencyId);
  }

  // Delete currency
  private deleteCurrency(currencyId: string | undefined): void {
    if(currencyId == null || currencyId == undefined) {
      this.toastrService.error("Currency is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.currencyService.delete(currencyId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Currency deleted successfully.", "Success"); 
        this.getCurrencies();
      } else {
        this.toastrService.error("Currency is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Currency is not deleted. Please, try again.", "Error");
      return;
    });
  }
}