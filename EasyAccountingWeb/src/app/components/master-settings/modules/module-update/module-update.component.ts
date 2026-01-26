import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ModuleService, ModuleUpdateModel, ModuleViewModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-module-update',
  templateUrl: './module-update.component.html',
  styleUrls: ['./module-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, RouterLink],
  providers: [ModuleService]
})

export class ModuleUpdateComponent implements OnInit {

  // Module update model
  moduleUpdateModel: ModuleUpdateModel = new ModuleUpdateModel();

  // Get module id
  private _moduleId: string | undefined;

  constructor(private moduleService: ModuleService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    // Get module id by url
    this.getModuleIdByUrl();
  }

  // Get module id by url
  private getModuleIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._moduleId = params["recordId"];

      // Get module by id
      if (this._moduleId != undefined || this._moduleId != null || this._moduleId! != "") {
        this.getModuleById();
      }
    });
  }

  // Get module by id
  private getModuleById(): void {
    this.spinnerService.show();
    this.moduleService.getById(this._moduleId!).subscribe((result: ModuleViewModel) => {
      this.moduleUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Module cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Module update form validation
  private getModuleUpdateFromValidationResult(): boolean {

    if (!this.moduleUpdateModel.name || this.moduleUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide module name.', 'Warning.');
      return false;
    }
    
    return true;
  }

  // On click update module
  onClickUpdateModule(): void {

    // Get module update form validation result
    let getUpdateFormValidation: boolean = this.getModuleUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.moduleService.update(this.moduleUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Module update successful.", "Successful");
          return this.router.navigateByUrl("/app/modules");
        } else {
          this.toastrService.error("Module cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Module cannot update successful.", "Error");
        return;
      })
    }
  }
}