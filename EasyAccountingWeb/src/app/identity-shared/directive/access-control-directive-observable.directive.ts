import { computed, Directive, effect, inject, signal } from '@angular/core';
import { IdentityService } from '../identity.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeFeatureActionDetailsModel, UserModel } from '../../../api/base-api';
import { catchError, filter, Observable, of, tap } from 'rxjs';

@Directive({
  selector: '[appAccessControlDirectiveObservable]'
})

export class AccessControlDirectiveObservableDirective {

  private _permissionsLoaded = signal(false);

  readonly permissionsLoaded = computed(() => this._permissionsLoaded());

  // Using inject() for DI
  private identityService = inject(IdentityService);
  private spinner = inject(NgxSpinnerService);

  constructor() {
    // Effect with allowSignalWrites enabled
    effect(
      () => {
        const perms = this._userAccessMappingDetails();
        // Writing to a signal inside an effect (allowed now)
        this._permissionsLoaded.set(perms.length > 0);
      },
      { allowSignalWrites: true }
    );
  }

  /**
  * Load permissions from server once
  */
  private readonly _userAccessMappingDetails = signal<EmployeeFeatureActionDetailsModel[]>([]);

  readonly hasEmployeeListAccess = computed(() =>
    this._userAccessMappingDetails().some(
      (x) => x.featureName === "Employee" && x.actionName === "List"
    )
  );

  loadPermissions$(): Observable<UserModel> {
    return this.identityService.getLoginInfo().pipe(
      filter((res): res is UserModel => res !== null),
      tap((res) => {
        this._userAccessMappingDetails.set(res.employeeFeatureActions ?? []);
      }),
      catchError(() => {
        this._userAccessMappingDetails.set([]);

        const user = new UserModel();
        user.employeeFeatureActions = [];

        return of(user);
      })
    );
  }

  /**
  * Generic access checker
  */
  hasAccess(feature: string, action: string | string[]): boolean {
    const mappings = this._userAccessMappingDetails();

    if (!mappings || mappings.length === 0) return false;

    if (Array.isArray(action)) {
      return action.some((a) =>
        mappings.some((m) => m.featureName === feature && m.actionName === a)
      );
    }

    return mappings.some((m) => m.featureName === feature && m.actionName === action);
  }
}