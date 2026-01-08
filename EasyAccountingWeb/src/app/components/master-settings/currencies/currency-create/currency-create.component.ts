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
import { CurrencyCreateModel, CurrencyService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';

@Component({
  selector: 'app-currency-create',
  templateUrl: './currency-create.component.html',
  styleUrls: ['./currency-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule, NzInputNumberModule],
  providers: [CurrencyService]
})

export class CurrencyCreateComponent implements OnInit {

  // Currency create model
  currencyCreateModel: CurrencyCreateModel = new CurrencyCreateModel();

  constructor(private currencyService: CurrencyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() { }
  
  // Currency create form validation
  private getCurrencyCreateFromValidationResult(): boolean {

    // Currency name validation
    if (!this.currencyCreateModel.name || this.currencyCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide currency name.', 'Warning.');
      return false;
    }

    // Currency base rate validation
    if (!this.currencyCreateModel.baseRate || this.currencyCreateModel.baseRate <= 0) {
      this.toastrService.warning('Please, provide base rate.', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // on click save currency
  onClickSaveCurrency(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getCurrencyCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.currencyService.create(this.currencyCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Currency create successful.", "Successful");
          return this.router.navigateByUrl("/app/currencies");
        } else {
          this.toastrService.error("Currency cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Currency is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}