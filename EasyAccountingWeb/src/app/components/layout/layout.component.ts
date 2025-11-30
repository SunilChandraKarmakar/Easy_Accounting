import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  standalone: true,
  imports: [HeaderComponent, SidebarComponent, FooterComponent, RouterOutlet]
})

export class LayoutComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}