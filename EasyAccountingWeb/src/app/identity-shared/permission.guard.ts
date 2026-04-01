import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AccessControlService } from "./services/access-control.service";
import { ToastrService } from "ngx-toastr";

@Injectable({ providedIn: "root" })

export class PermissionGuard implements CanActivate {

  constructor(
    private accessControlService: AccessControlService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const permission = route.data["permission"];

    if (permission && this.accessControlService.hasAccess(permission.feature, permission.action)) {
      return true;
    }

    // this.toastr.warning("You don't have permission to access this page.", "Access Denied");
    this.router.navigateByUrl("/app/dashboard");
    return false;
  }
}