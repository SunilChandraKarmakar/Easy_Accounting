import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { LoginModel } from '../../../api/base-api';
import { ToastrService } from 'ngx-toastr';
import { IdentityService } from '../../identity-shared/identity.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NgxSpinnerModule]
})

export class LoginComponent implements OnInit {

  // Show hide password
  isShowPassword: boolean = false;

  // Login model
  loginModel: LoginModel = new LoginModel();

  constructor(private spinnerService: NgxSpinnerService, private toastrService: ToastrService, private identityService: IdentityService, private router: Router) { }

  ngOnInit() { }

  // Toggle password visibility
  togglePassword() {
    this.isShowPassword = !this.isShowPassword;
  }

  // Get login form validation result
  private getLoginFormValidationResult(): boolean {
    if(this.loginModel.email == undefined || this.loginModel.email == null || this.loginModel.email.trim() == '') {
      this.toastrService.warning('Email is required.', 'Validation Warning');
      return false;
    }
    else if(this.loginModel.password == undefined || this.loginModel.password == null || this.loginModel.password.trim() == '') {
      this.toastrService.warning('Password is required.', 'Validation Warning');
      return false;
    }
    else {
      return true;
    }
  }

  // Handle login button click
  async onClickLogin(): Promise<void> {

   const isLoginFormValid: boolean = this.getLoginFormValidationResult();

    if (isLoginFormValid) {
      this.spinnerService.show();

      // Get the full login result (with isSuccess and message)
      const loginResult = await this.identityService.SignIn(this.loginModel);
      this.spinnerService.hide();

      if (loginResult.isSuccess) {
        this.toastrService.success("Welcome back! You’re now logged in.", "Success");
        this.router.navigateByUrl("/app/countries");
      } else {
        console.log(loginResult.message);
        this.toastrService.error(loginResult.message ?? "Login failed. Please try again.", "Error",  { enableHtml: true, timeOut: 10000, closeButton: true });
        return;
      }
    }
  }
}