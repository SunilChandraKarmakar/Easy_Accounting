import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { IdentityService } from "./identity.service";
import { ToastrService } from "ngx-toastr";
import { map, Observable } from "rxjs";
import { UserModel } from "../../api/base-api";

@Injectable({ providedIn: 'root' })

export class AuthGuard implements CanActivate {
  constructor(private router: Router, private identityService: IdentityService, private tasterService: ToastrService) { }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    return this.identityService.getLoginInfo().pipe(map((user: UserModel) => {
        if (user != null) {
          return true;
        } else {
          this.tasterService.warning("You must be logged in to access this resource.", "Warning");
          this.router.navigateByUrl("/login");
          return false;
        }
      })
    );
  }
}