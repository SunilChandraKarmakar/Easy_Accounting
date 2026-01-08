import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CurrencyService, CurrencyUpdateModel, CurrencyViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-currency-update',
  templateUrl: './currency-update.component.html',
  styleUrls: ['./currency-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, RouterLink, NzInputNumberModule],
  providers: [CurrencyService]
})

export class CurrencyUpdateComponent implements OnInit {

  // Currency update model
  currencyUpdateModel: CurrencyUpdateModel = new CurrencyUpdateModel();

  // Get currency id
  private _currencyId: string | undefined;

  constructor(private currencyService: CurrencyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // Get currency id by url
    this.getCurrencyIdByUrl();
  }

  // Get currency id by url
  private getCurrencyIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._currencyId = params["recordId"];

      // Get currency by id
      if (this._currencyId != undefined || this._currencyId != null || this._currencyId! != "") {
        this.getCurrencyById();
      }
    });
  }

  // Get currency by id
  private getCurrencyById(): void {
    this.spinnerService.show();
    this.currencyService.getById(this._currencyId!).subscribe((result: CurrencyViewModel) => {
      this.currencyUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("CUrrency cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Currency update form validation
  private getCurrencyUpdateFromValidationResult(): boolean {

    // Currency name validation
    if (!this.currencyUpdateModel.name || this.currencyUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide currency name.', 'Warning.');
      return false;
    }

    // Currency base rate validation
    if (!this.currencyUpdateModel.baseRate || this.currencyUpdateModel.baseRate <= 0) {
      this.toastrService.warning('Please, provide base rate.', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // On click update currency
  onClickUpdateCurrency(): void {

    // Get currency update form validation result
    let getUpdateFormValidation: boolean = this.getCurrencyUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.currencyService.update(this.currencyUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Currency update successful.", "Successful");
          return this.router.navigateByUrl("/app/currencies");
        } else {
          this.toastrService.error("Currency cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Currency cannot update successful.", "Error");
        return;
      })
    }
  }

  cancel(): void { }
}