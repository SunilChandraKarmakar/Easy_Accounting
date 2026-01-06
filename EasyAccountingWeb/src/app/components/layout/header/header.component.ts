import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { IdentityService } from '../../../identity-shared/identity.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {

  constructor(private spinnerService: NgxSpinnerService, private toastrService: ToastrService, private identityService: IdentityService, private router: Router) { }

  ngOnInit() {
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
}
