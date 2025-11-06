import { Component, OnInit } from '@angular/core';
import { NzButtonModule } from 'ng-zorro-antd/button';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.css'],
  standalone: true,
  imports: [NzButtonModule]
})
export class CountriesComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
