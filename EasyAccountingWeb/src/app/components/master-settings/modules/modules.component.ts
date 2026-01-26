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
import { FilterPageModel, FilterPageResultModelOfModuleGridModel, ModuleGridModel, ModuleService } from '../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-modules',
  templateUrl: './modules.component.html',
  styleUrls: ['./modules.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule],
  providers: [ModuleService]
})

export class ModulesComponent implements OnInit {

  // Table property
  modules: ModuleGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();
  
  constructor(private moduleService: ModuleService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get modules
    this.getModules();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get modules
  private getModules(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.modules = [];
    this.totalRecord = 0;

    this.moduleService.getFilterModules(this.filterPageModel).subscribe((result: FilterPageResultModelOfModuleGridModel) => {
      this.modules = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.modules = [];
      this.totalRecord = 0;

      this.toastrService.error("Module list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get modules
    this.getModules();
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

    // Get modules
    this.getModules();
  }

  // On click open delete modal
  onClickDelete(moduleId: string | undefined): void {
    this.deleteModule(moduleId);
  }

  // Delete module
  private deleteModule(moduleId: string | undefined): void {
    if(moduleId == null || moduleId == undefined) {
      this.toastrService.error("Module is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.moduleService.delete(moduleId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Module deleted successfully.", "Success"); 
        this.getModules();
      } else {
        this.toastrService.error("Module is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Module is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}