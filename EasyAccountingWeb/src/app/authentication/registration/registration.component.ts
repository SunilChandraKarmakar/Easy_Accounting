import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../../../api/base-api';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
})

export class RegistrationComponent implements OnInit {

  registerModel: RegisterModel = new RegisterModel();

  constructor() {}

  ngOnInit(): void {
  }
}