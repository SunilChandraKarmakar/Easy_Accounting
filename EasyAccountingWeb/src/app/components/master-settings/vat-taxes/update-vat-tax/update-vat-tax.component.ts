import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { SelectModel, VatTaxService, VatTaxUpdateModel, VatTaxViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-update-vat-tax',
  templateUrl: './update-vat-tax.component.html',
  styleUrls: ['./update-vat-tax.component.css'],
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

export class UpdateVatTaxComponent implements OnInit {

  // Default vat tax id
  private _vatTaxId: string = "-1";

  // Select list
  companies: SelectModel[]  = [];

  // VatTax update model
  vatTaxUpdateModel: VatTaxUpdateModel = new VatTaxUpdateModel();

  constructor(
    private vatTaxService: VatTaxService, 
    private spinnerService: NgxSpinnerService, 
    private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    // Get vat tax id by url
    this.getVatTaxIdByUrl();
  }

  // Get vat tax id by url
  private getVatTaxIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._vatTaxId = params["recordId"];

      if (this._vatTaxId != undefined || this._vatTaxId != null || this._vatTaxId! != "") {
        this.getVatTaxById(this._vatTaxId);
      }
    });
  }

  // Get vat tax by id
  private getVatTaxById(vatTaxId: string): void {
    this.spinnerService.show();
    this.vatTaxService.getById(vatTaxId).subscribe((result: VatTaxViewModel) => {

      this.vatTaxUpdateModel = result.updateModel!;

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

  // Vat tax update form validation
  private getVatTaxUpdateFromValidationResult(): boolean {

    if (this.vatTaxUpdateModel.taxName == undefined || this.vatTaxUpdateModel.taxName == null || this.vatTaxUpdateModel.taxName == "") {
      this.toastrService.warning('Please, provide tax name.', 'Warning.');
      return false;
    } else if (this.vatTaxUpdateModel.taxNumber == undefined || this.vatTaxUpdateModel.taxNumber == null 
      || this.vatTaxUpdateModel.taxNumber == "") {
      this.toastrService.warning('Please, provide tax number.', 'Warning.');
      return false;
    } else if (this.vatTaxUpdateModel.rate == undefined || this.vatTaxUpdateModel.rate == null 
      || this.vatTaxUpdateModel.rate < 0) {
      this.toastrService.warning('Please, provide rate.', 'Warning.');
      return false;
    } else if (this.vatTaxUpdateModel.companyId == undefined || this.vatTaxUpdateModel.companyId == null 
      || this.vatTaxUpdateModel.companyId <= 0) {
      this.toastrService.warning('Please, select company.', 'Warning.');
      return false;
    } else {
      return true;
    }
  }

  // on click update vat tax
  onClickUpdateVatTax(): void {

    // Get update from validation result
    let getUpdateFormValidationResult: boolean = this.getVatTaxUpdateFromValidationResult();

    if(getUpdateFormValidationResult) {
      this.spinnerService.show();
      this.vatTaxService.create(this.vatTaxUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Vat Tax update successful.", "Successful");
          return this.router.navigateByUrl("/app/vat-taxes");
        } else {
          this.toastrService.error("Vat Tax cannot updated! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Vat Tax is not updated! Please, try again.", "Error.");
        return;
      })
    }
  }
}