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

  openMenus: { [key: string]: boolean } = {};

  menuMapping: { [key: string]: string[] } = {
    users: ['users', 'user-groups', 'permissions'],
    userGroups: ['user-groups', 'permissions'],
    permissions: ['role-management'],
    settings: ['countries', 'cities']
  };

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.checkCurrentUrl());
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
      this.openMenus[menuKey] = keywords.some(k => url.includes(k));
    }
  }
}
