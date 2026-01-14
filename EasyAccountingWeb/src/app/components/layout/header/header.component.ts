import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IdentityService } from '../../../identity-shared/identity.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { UserModel } from '../../../../api/base-api';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {

  // Login user info model
  loginUserInfoModel: UserModel = new UserModel();

  constructor(private spinnerService: NgxSpinnerService, private toastrService: ToastrService, private identityService: IdentityService, private router: Router) { }

  ngOnInit() {

    // Get login user info
    this.getLoginUserInfo();
  }

  async onClickLogout(): Promise<void> {
    this.spinnerService.hide();
    let getLoginResult: boolean = await this.identityService.logout();

    if (getLoginResult) {
      this.toastrService.success("You have been logged out successfully.", "Success");
      this.router.navigateByUrl("/login");
    } else {
      this.spinnerService.hide();
      this.toastrService.error("Logout cannot Success.! Please, try again.", "Wrong");
      return;
    }
  }

  // Get login user info
  private getLoginUserInfo(): void {
    this.spinnerService.show();
    this.identityService.getLoginInfo().subscribe((result: UserModel) => {
      this.loginUserInfoModel = result;
      this.spinnerService.hide();
      return;
    },
    (error: any) => {
      this.spinnerService.hide();
      this.toastrService.error("Login user information cannot found! Please, try again.", "Error.");
      return;
    })
  }
}