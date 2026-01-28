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
import { ActionGridModel, ActionService, FilterPageModel, FilterPageResultModelOfActionGridModel } from '../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-actions',
  templateUrl: './actions.component.html',
  styleUrls: ['./actions.component.css'],
  standalone: true,
  imports: [CommonModule, NzButtonModule, NzDividerModule, NzTableModule, RouterLink, NgxSpinnerModule, NzSpaceModule, NzInputModule, 
    NzIconModule, NzBreadCrumbModule, NzPopconfirmModule],
  providers: [ActionService]
})

export class ActionsComponent implements OnInit {

  // Table property
  actions: ActionGridModel[] = [];
  totalRecord: number = 0;

  // Filter page model
  filterPageModel: FilterPageModel = new FilterPageModel();
  
  constructor(private actionService: ActionService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
    // Initialize page filter model
    this.initializeFilterModel();

    // Get actions
    this.getActions();
  }

  // Initialize filter model
  private initializeFilterModel(): void {
    this.filterPageModel.pageIndex = 0;
    this.filterPageModel.pageSize = 10;
    this.filterPageModel.sortColumn = "name";
    this.filterPageModel.sortOrder = "ascend"
    this.filterPageModel.filterValue = "";
  }

  // Get actions
  private getActions(): void {
    this.spinnerService.show();

    // Clear before loading 
    this.actions = [];
    this.totalRecord = 0;

    this.actionService.getFilterActions(this.filterPageModel).subscribe((result: FilterPageResultModelOfActionGridModel) => {
      this.actions = result.items || [];
      this.totalRecord = result.totalCount || 0;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();

      // Keep cleared state on error
      this.actions = [];
      this.totalRecord = 0;

      this.toastrService.error("Action list is not show at this time! Please, try again.", "Error");
      return;
    });
  }

  onChangeApplyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filterPageModel.filterValue = filterValue;
    this.filterPageModel.pageIndex = 0;

    // Get actions
    this.getActions();
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

    // Get actions
    this.getActions();
  }

  // On click open delete modal
  onClickDelete(actionId: string | undefined): void {
    this.deleteAction(actionId);
  }

  // Delete action
  private deleteAction(actionId: string | undefined): void {
    if(actionId == null || actionId == undefined) {
      this.toastrService.error("Action is not found. Please, try again.", "Error");
      return;
    }

    this.spinnerService.show();
    this.actionService.delete(actionId).subscribe((result: boolean) => {
      this.spinnerService.hide();
      if(result) {
        this.toastrService.success("Action deleted successfully.", "Success"); 
        this.getActions();
      } else {
        this.toastrService.error("Action is not deleted. Please, try again.", "Error");
      }

      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Action is not deleted. Please, try again.", "Error");
      return;
    });
  }

  cancel(): void { }
}