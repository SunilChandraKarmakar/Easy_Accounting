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
import { ModuleCreateModel, ModuleService } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-module-create',
  templateUrl: './module-create.component.html',
  styleUrls: ['./module-create.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, 
    NzDividerModule],
  providers: [ModuleService]
})

export class ModuleCreateComponent implements OnInit {

  // Module create model
  moduleCreateModel: ModuleCreateModel = new ModuleCreateModel();

  constructor(private moduleService: ModuleService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() { }

  // Module create form validation
  private getModuleCreateFromValidationResult(): boolean {

    if (!this.moduleCreateModel.name || this.moduleCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide module name.', 'Warning.');
      return false;
    }

    return true;
  }

  // on click save module
  onClickSaveModule(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getModuleCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.moduleService.create(this.moduleCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Module create successful.", "Successful");
          return this.router.navigateByUrl("/app/modules");
        } else {
          this.toastrService.error("Module cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Module is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}