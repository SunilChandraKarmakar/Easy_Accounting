import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ActionService, ActionUpdateModel, ActionViewModel } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-action-update',
  templateUrl: './action-update.component.html',
  styleUrls: ['./action-update.component.css'],
  standalone: true,
  imports: [FormsModule, CommonModule, NzButtonModule, NgxSpinnerModule, NzInputModule, NzIconModule, NzBreadCrumbModule, RouterLink],
  providers: [ActionService]
})

export class ActionUpdateComponent implements OnInit {

  // Action update model
  actionUpdateModel: ActionUpdateModel = new ActionUpdateModel();

  // Get action id
  private _actionId: string | undefined;

  constructor(private actionService: ActionService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

    // Get action id by url
    this.getActionIdByUrl();
  }

  // Get action id by url
  private getActionIdByUrl(): void {
    this.activatedRoute.params.subscribe((params) => {
      this._actionId = params["recordId"];

      // Get action by id
      if (this._actionId != undefined || this._actionId != null || this._actionId! != "") {
        this.getActionById();
      }
    });
  }
  
  // Get action by id
  private getActionById(): void {
    this.spinnerService.show();
    this.actionService.getById(this._actionId!).subscribe((result: ActionViewModel) => {
      this.actionUpdateModel = result.updateModel!;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Action cannot found! Please, try again.", "Error");
      return;
    })
  }

  // Action update form validation
  private getActionUpdateFromValidationResult(): boolean {

    if (!this.actionUpdateModel.name || this.actionUpdateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide action name.', 'Warning.');
      return false;
    }
    
    return true;
  }

  // On click update action
  onClickUpdateAction(): void {

    // Get action update form validation result
    let getUpdateFormValidation: boolean = this.getActionUpdateFromValidationResult();

    if(getUpdateFormValidation) {
      this.spinnerService.show();
      this.actionService.update(this.actionUpdateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Action update successful.", "Successful");
          return this.router.navigateByUrl("/app/actions");
        } else {
          this.toastrService.error("Action cannot update successful.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Action cannot update successful.", "Error");
        return;
      })
    }
  }
}