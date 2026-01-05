import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [RouterLink],
  providers: []
})

export class LoginComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
