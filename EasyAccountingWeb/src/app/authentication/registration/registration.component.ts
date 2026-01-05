import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../../../api/base-api';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { IdentityService } from '../../identity-shared/identity.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, FormsModule, CommonModule, NgxSpinnerModule]
})

export class RegistrationComponent implements OnInit {

  registerModel: RegisterModel = new RegisterModel();

  constructor(private spinnerService: NgxSpinnerService, private toastrService: ToastrService, private identityService: IdentityService) { }

  ngOnInit(): void {

  }

  // Get registration form validation result
  private getRegistrationFormValidationResult(): boolean {
    if(this.registerModel.fullName == undefined || this.registerModel.fullName == null || this.registerModel.fullName.trim() == '') {
      this.toastrService.warning('Full Name is required.', 'Validation Warning');
      return false;
    }
    else if(this.registerModel.email == undefined || this.registerModel.email == null || this.registerModel.email.trim() == '') {
      this.toastrService.warning('Email is required.', 'Validation Warning');
      return false;
    }
    else if(this.registerModel.phone == undefined || this.registerModel.phone == null || this.registerModel.phone.trim() == '') {
      this.toastrService.warning('Phone is required.', 'Validation Warning');
      return false;
    }
    else if(this.registerModel.password == undefined || this.registerModel.password == null || this.registerModel.password.trim() == '') {
      this.toastrService.warning('Password is required.', 'Validation Warning');
      return false;
    }
    else if(this.registerModel.confirmPassword == undefined || this.registerModel.confirmPassword == null 
      || this.registerModel.confirmPassword.trim() == '') {
      this.toastrService.warning('Confirm Password is required.', 'Validation Warning');
      return false;
    }
    else if(this.registerModel.password != this.registerModel.confirmPassword) {
      this.toastrService.warning('Password and Confirm Password do not match.', 'Validation Warning');
      return false;
    }
    else {
      return true;
    }
  }

  // Handle registration button click
  onClickRegistration(): void {

    if(this.getRegistrationFormValidationResult()) {
      this.spinnerService.show();
      this.identityService.SignUp(this.registerModel);
    }
  }
}