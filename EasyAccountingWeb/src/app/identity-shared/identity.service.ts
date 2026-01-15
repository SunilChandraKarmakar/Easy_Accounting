import { Injectable, Inject, PLATFORM_ID } from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { BehaviorSubject, Observable, of } from "rxjs";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerService } from "ngx-spinner";
import { HttpBackend, HttpClient, HttpHeaders } from "@angular/common/http";
import { lastValueFrom } from "rxjs";
import { AuthenticationService, AuthenticationViewModel, LoginModel, RegisterModel, UserModel } from "../../api/base-api";

@Injectable({ providedIn: "root" })

export class IdentityService {

  private readonly _jwtToken = "jwt_token";
  private readonly _loginUserInfo = "login_user_info";
  private readonly _userIpInfo = "user_ip_info";

  private _cachedJwtToken: string | null = null;
  private _ipAddress: string | undefined;

  private readonly _currentUserSubject = new BehaviorSubject<UserModel | null>(null);
  public readonly currentUser$ = this._currentUserSubject.asObservable();

  constructor(private authenticationService: AuthenticationService, private router: Router, private toasterService: ToastrService, private spinnerService: NgxSpinnerService,
    private httpBackend: HttpBackend, @Inject(PLATFORM_ID) private platformId: Object) {

    // Use raw HttpClient (no interceptors) for IP call if needed
    const rawHttp = new HttpClient(this.httpBackend);

    // Load initial user from localStorage
    this._currentUserSubject.next(this.readUserFromStorage());

    // Get IP address
    this.getIPAddress(rawHttp);
  }

  // Helper: safe parse
  private readUserFromStorage(): UserModel | null {

    if (!isPlatformBrowser(this.platformId)) return null;

    const raw = localStorage.getItem(this._loginUserInfo);
    if (!raw) return null;

    try {
      return JSON.parse(raw) as UserModel;
    } catch {
      return null;
    }
  }

  // Expose a simple sync boolean
  get isLoggedIn(): boolean {
    return !!this._currentUserSubject.value && this.getJwtToken() !== "No Token Found!";
  }

  // AuthGuard can use this (reactive)
  getLoginInfo(): Observable<UserModel | null> {
    return this.currentUser$;
  }

  // Get IP address
  private getIPAddress(rawHttp: HttpClient): void {
    const headers = new HttpHeaders({
      "Content-Type": "application/json; charset=UTF-8"
    });

    if (!this._ipAddress) {
      rawHttp.get("https://api.ipify.org/?format=json", { headers }).subscribe({
        next: (res: any) => {
          
          this._ipAddress = res.ip;
          
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem(this._userIpInfo, res.ip);
          }
        },
        error: () => { /* ignore */ }
      });
    }
  }

  // Sign in operation
  async SignIn(model: LoginModel): Promise<{ isSuccess: boolean; message?: string }> {

    this.spinnerService.show();
    
    try {

      const result: AuthenticationViewModel = await lastValueFrom(this.authenticationService.login(model));
      this.spinnerService.hide();
      this.storeJwtTokens(result);

      // Update current user stream
      this._currentUserSubject.next(result.userModel ?? null);
      return { isSuccess: true, message: "Login Success." };

    } catch (error: any) {

      this.spinnerService.hide();
      const errorMessage: string = this.getValidationErrors(error);
      return { isSuccess: false, message: errorMessage };
    }
  }

  // Sign up operation
  SignUp(model: RegisterModel): void {

    this.spinnerService.show();

    this.authenticationService.registration(model).subscribe({
      next: (result: UserModel) => {
        this.spinnerService.hide();
        this.toasterService.success("Account has been created successfully. Please sign in to continue.", "Success");
        this.router.navigateByUrl("/login");
      },
      error: (error: any) => {

        this.spinnerService.hide();
        const errorMessage: string = this.getValidationErrors(error);
        this.toasterService.error(errorMessage, "Validation Error", { enableHtml: true, timeOut: 10000, closeButton: true });
      }
    });
  }

  async logout(): Promise<boolean> {
    try {

      this.removeTokens();

      // Update auth state
      this._currentUserSubject.next(null);
      await this.router.navigateByUrl("/login");
      return true;

    } catch {
      return false;
    }
  }

  private storeJwtTokens(identityViewModel: AuthenticationViewModel): void {
    this.removeTokens();

    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this._jwtToken, identityViewModel.userModel!.token!);
      localStorage.setItem(this._loginUserInfo, JSON.stringify(identityViewModel.userModel));
      if (this._ipAddress) localStorage.setItem(this._userIpInfo, this._ipAddress);
    }

    // Refresh cached token
    this._cachedJwtToken = identityViewModel.userModel?.token ?? null;
  }

  private removeTokens(): void {
    this._cachedJwtToken = null;

    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this._jwtToken);
      localStorage.removeItem(this._loginUserInfo);
      localStorage.removeItem(this._userIpInfo);
    }
  }

  getJwtToken(): string {
    if (!this._cachedJwtToken && isPlatformBrowser(this.platformId)) {
      this._cachedJwtToken = localStorage.getItem(this._jwtToken);
    }
    return this._cachedJwtToken || "No Token Found!";
  }

  private getValidationErrors(error: any): string {
    if (!error?.errors) {
      return error?.errorMessage ?? "Something went wrong.";
    }

    const validationErrors = error.errors;
    const messages: string[] = [];

    for (const field in validationErrors) {
      if (validationErrors.hasOwnProperty(field)) {
        validationErrors[field].forEach((message: string) => {
          messages.push(`${field}: ${message}`);
        });
      }
    }

    return messages.join("<br><br>");
  }
}