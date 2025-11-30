import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, NavigationEnd } from "@angular/router";
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [RouterLink, CommonModule]
})

export class SidebarComponent implements OnInit {

  openMenus: { [key: string]: boolean } = {};

  menuMapping: { [key: string]: string[] } = {
    'settings':   ['countries', 'cities', 'currencies', 'languages'],
    'users':      ['users', 'roles', 'permissions'],
    'userGroups': ['user-groups', 'permissions'],
    'reports':    ['sales-report', 'audit-log']  
  };

  constructor(private router: Router) { 
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.checkCurrentUrl();
    });
  }

  ngOnInit() {
    this.checkCurrentUrl();
  }

  toggleMenu(key: string) {
    this.openMenus[key] = !this.openMenus[key];
  }

  checkCurrentUrl() {
    const url = this.router.url;

    for (const [menuKey, keywords] of Object.entries(this.menuMapping)) {
      if (keywords.some(keyword => url.includes(keyword))) {
        this.openMenus[menuKey] = true;
      }
    }
  }
}