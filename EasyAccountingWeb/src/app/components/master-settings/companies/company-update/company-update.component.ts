import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CompanyService, CompanyUpdateModel, CompanyViewModel, SelectModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-company-update',
  templateUrl: './company-update.component.html',
  styleUrls: ['./company-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, RouterLink, NzInputNumberModule],
  providers: [CompanyService]
})

export class CompanyUpdateComponent implements OnInit {

  // Company update model
  companyUpdateModel: CompanyUpdateModel = new CompanyUpdateModel();

  // Select list
  countries: SelectModel[] = [];
  cities: SelectModel[] = [];
  currencies: SelectModel[] = [];

  // Get company id
  private _companyId: string | undefined;

  constructor(private companyService: CompanyService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    // Get company id by url
    this.getCompanyIdByUrl();
  }

  // Get company id by url
  private getCompanyIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._companyId = params["recordId"];

      // Get company by id
      if (this._companyId != undefined || this._companyId != null || this._companyId! != "") {
        this.getCompanyById();
      }
    });
  }

  // Get company by id
  private getCompanyById(): void {
    this.spinnerService.show();
    this.companyService.getById(this._companyId!).subscribe((result: CompanyViewModel) => {
      this.companyUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Company cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Company update form validation
  private getCompanyUpdateFromValidationResult(): boolean {

    // Company name validation
    if (!this.companyUpdateModel.name || this.companyUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide company name.', 'Warning.');
      return false;
    }

    // Company phone number validation
    if (!this.companyUpdateModel.phone || this.companyUpdateModel.phone.trim() === '') {
      this.toastrService.warning('Please, provide company phone number.', 'Warning.');
      return false;
    }

    // Company country validation
    if (this.companyUpdateModel.countryId == undefined || this.companyUpdateModel.countryId == null || this.companyUpdateModel.countryId <= 0) {
      this.toastrService.warning('Please, provide country', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyUpdateModel.cityId == undefined || this.companyUpdateModel.cityId == null || this.companyUpdateModel.cityId <= 0) {
      this.toastrService.warning('Please, provide city', 'Warning.');
      return false;
    }

    // Company city validation
    if (this.companyUpdateModel.currencyId == undefined || this.companyUpdateModel.currencyId == null || this.companyUpdateModel.currencyId <= 0) {
      this.toastrService.warning('Please, provide currency', 'Warning.');
      return false;
    }

    // All validations passed
    return true;
  }

  // On click update company
  onClickUpdateCompany(): void {

    // Get company update form validation result
    let getUpdateFormValidation: boolean = this.getCompanyUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.companyService.update(this.companyUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Company update successful.", "Successful");
          return this.router.navigateByUrl("/app/companies");
        } else {
          this.toastrService.error("Company cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Company cannot update successful.", "Error");
        return;
      })
    }
  }

  cancel(): void { }
}