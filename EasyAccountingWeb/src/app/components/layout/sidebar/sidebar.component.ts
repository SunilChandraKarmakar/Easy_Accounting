import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { filter } from 'rxjs/operators';
import { IdentityService } from '../../../identity-shared/identity.service';
import { SidebarPermissionService } from '../../../identity-shared/services/sidebar-permission.service';
import { AccessControlService } from '../../../identity-shared/services/access-control.service';
import { UserModel } from '../../../../api/base-api';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})

export class SidebarComponent implements OnInit {

  // Login user info
  loginUserInfo: UserModel = new UserModel();

  private initialized = false;
  openMenus: { [key: string]: boolean } = {};

  menuMapping: { [key: string]: string[] } = {
    users: ['users', 'user-groups', 'permissions'],
    userGroups: ['user-groups', 'permissions'],
    permissions: ['role-management'],
    settings: ['countries', 'cities', 'currencies', 'companies', 'invoice-settings', 'modules', 'vat-taxes', 
      'product-units', 'vendors'], 
    accessControl: ['actions', 'features', 'feature-actions', 'employee-feature-actions']
  };

  constructor(
    public accessControlService: AccessControlService,
    private identityService: IdentityService,
    public sidebarPermissionService: SidebarPermissionService,
    private router: Router,
    private cdr: ChangeDetectorRef) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => this.checkCurrentUrl());
  }

  ngOnInit() {
    Object.keys(this.menuMapping).forEach(key => {
      this.openMenus[key] = false;
    });

    this.checkCurrentUrl();

    this.accessControlService.setPermissions();
    this.getLoginUserInfo();
  }

  // Get login user info
  private getLoginUserInfo(): void {
    this.identityService.getLoginInfo()
      .pipe(filter((res): res is UserModel => res !== null))
      .subscribe((result) => {
        this.loginUserInfo = result;
        this.sidebarPermissionService.initialize(this.loginUserInfo);
        this.cdr.detectChanges();
      });
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