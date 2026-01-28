import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})

export class SidebarComponent implements OnInit {

  private initialized = false;
  openMenus: { [key: string]: boolean } = {};

  menuMapping: { [key: string]: string[] } = {
    users: ['users', 'user-groups', 'permissions'],
    userGroups: ['user-groups', 'permissions'],
    permissions: ['role-management'],
    settings: ['countries', 'cities', 'currencies', 'companies', 'invoice-settings', 'modules'], 
    accessControl: ['actions', 'features', 'user-permission']
  };

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.checkCurrentUrl());
  }

  ngOnInit() {
    Object.keys(this.menuMapping).forEach(key => {
      this.openMenus[key] = false;
    });

    this.checkCurrentUrl();
  }

  toggleMenu(key: string) {
    this.openMenus[key] = !this.openMenus[key];
  }

  checkCurrentUrl() {
    if (this.initialized) return;

    const url = this.router.url.toLowerCase();

    Object.keys(this.openMenus).forEach(key => {
      this.openMenus[key] = false;
    });

    for (const [menuKey, keywords] of Object.entries(this.menuMapping)) {
      if (keywords.some(k => url.includes(k))) {
        this.openMenus[menuKey] = true;
      }
    }

    this.initialized = true;
  }
}