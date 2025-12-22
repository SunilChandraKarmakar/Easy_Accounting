import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { CountryCreateModel, CountryService } from '../../../../../../api/base-api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-country-create',
  templateUrl: './country-create.component.html',
  styleUrls: ['./country-create.component.css'],
  standalone: true,
  imports: [NzButtonModule, RouterLink, NgxSpinnerModule, NzInputModule, NzIconModule],
  providers: [CountryService]
})

export class CountryCreateComponent implements OnInit {

  // Country create model
  countryCreateModel: CountryCreateModel = new CountryCreateModel();

  constructor(private countryService: CountryService, private spinnerService: NgxSpinnerService, private toastrService: ToastrService) { }

  ngOnInit() {
  }

}