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
import { ActionCreateModel, ActionService } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-action-create',
  templateUrl: './action-create.component.html',
  styleUrls: ['./action-create.component.css'],
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
    NzDividerModule],
  providers: [ActionService]
})

export class ActionCreateComponent implements OnInit {

  // Action create model
  actionCreateModel: ActionCreateModel = new ActionCreateModel();

  constructor(private actionService: ActionService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService,
    private router: Router) { }

  ngOnInit() { }

  // Action create form validation
  private getActionCreateFromValidationResult(): boolean {

    if (!this.actionCreateModel.name || this.actionCreateModel.name.trim() === '') {
      this.toastrService.warning('Please, provide action name.', 'Warning.');
      return false;
    }

    return true;
  }

  // on click save action
  onClickSaveAction(): void {

    // Get create from validation result
    let getCreateFormValidationResult: boolean = this.getActionCreateFromValidationResult();

    if(getCreateFormValidationResult) {
      this.spinnerService.show();
      this.actionService.create(this.actionCreateModel).subscribe((result: boolean) => {
        this.spinnerService.hide();
        
        if(result) {
          this.toastrService.success("Action create successful.", "Successful");
          return this.router.navigateByUrl("/app/actions");
        } else {
          this.toastrService.error("Action cannot created! Please, try again.", "Error");
          return;
        }
      },
      (error: any) => {
        this.spinnerService.hide();
        this.toastrService.error("Action is not created! Please, try again.", "Error.");
        return;
      })
    }
  }
}