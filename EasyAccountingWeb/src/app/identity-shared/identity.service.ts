import { Inject, Injectable, PLATFORM_ID } from "@angular/core";
import { AuthenticationService, RegisterModel, UserModel } from "../../api/base-api";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { NgxSpinnerService } from "ngx-spinner";
import { HttpBackend, HttpClient, HttpHeaders } from "@angular/common/http";
import { isPlatformBrowser } from "@angular/common";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})

export class IdentityService {

    private readonly _jwtToken: string = "jwt_token";
    private readonly _refreshToken = "refresh_token";
    private readonly _loginUserInfo: string = "login_user_info";
    private readonly _userIpInfo: string = "user_ip_info";
    private _ipAddress: string | undefined;

  public isCodeVerify: boolean = true;
  get hasCodeVerified(): boolean {
    return this.isCodeVerify;
  }

  constructor(private authenticationService: AuthenticationService, private router: Router, private toasterService: ToastrService,
    private spinnerService: NgxSpinnerService, private httpClient: HttpClient, private httpBackend: HttpBackend,
    @Inject(PLATFORM_ID) private platformId: Object) {

    this.httpClient = new HttpClient(httpBackend);

    // Get IP address
    this.getIPAddress();
  }

  // Get IP address
  private getIPAddress(): void {
    
    const customHttpHeaders = new HttpHeaders();
    customHttpHeaders.append("Access-Control-Allow-Headers", "Content-Type");
    customHttpHeaders.append("Access-Control-Allow-Methods", "GET");
    customHttpHeaders.append("Access-Control-Allow-Origin", "*");
    customHttpHeaders.append("Content-Type", "application/json; charset=UTF-8");

    if (!this._ipAddress) {
        
      this.httpClient.get("https://api.ipify.org/?format=json", { headers: customHttpHeaders }).subscribe((res: any) => {
        this._ipAddress = res.ip;
        if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem(this._userIpInfo, res.ip);
        }
    });
    }
  }

  // Sign in operation
//   async SignIn(model: LoginModel): Promise<{ isSuccess: boolean; message?: string }> {
//     this.getIPAddress();

//     this.spinnerService.show();

//     try {
//       const result: IdentityViewModel = await lastValueFrom(this.authenticationService.login(model));
//       this.spinnerService.hide();
      
//       this.storeJwtTokens(result);
      
//       if (result.userModel.forcePasswordChanged == false) {
//         return { isSuccess: true, message: "Change Password." };
//       }
      
//       return { isSuccess: true, message: "Login Success." };
//     } catch (error: any) {
//       this.spinnerService.hide();
//       return { isSuccess: false, message: error.errorMessage };
//     }
//   }

  async SignUp(model: RegisterModel) {
    this.spinnerService.show();
    this.authenticationService.registration(model).subscribe(async (result: UserModel) => {
        this.spinnerService.hide();
        this.router.navigateByUrl("/app/countries");
        return this.toasterService.success("Your account has been created successfully", "Success");
      },
      (err) => {
        this.spinnerService.hide();
        this.toasterService.error("Account cannot created! Please, try again.", "Error");
      }
    );
  }

  async logout(): Promise<boolean> {
    let logoutResult: boolean | undefined;

    try {
      this.removeTokens();
      logoutResult = true;
    } catch (error) {
      this.spinnerService.hide();
      logoutResult = false;
    }

    return logoutResult;
  }

  // Get login info
  getLoginInfo(): Observable<UserModel> {
    let loginUserInfo: string | undefined;
    let loginUser: UserModel = new UserModel();

    if (isPlatformBrowser(this.platformId)) {
      loginUserInfo = localStorage.getItem(this._loginUserInfo)!;
      loginUser = JSON.parse(loginUserInfo);
    }

    return new Observable((obs) => {
      obs.next(loginUser);
    });
  }

  // Set tokens
  private storeJwtTokens(identityViewModel: IdentityViewModel): void {
    this.removeTokens();

    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this._jwtToken, identityViewModel.userModel!.token!);
      localStorage.setItem(this._loginUserInfo, JSON.stringify(identityViewModel.userModel));
      localStorage.setItem(this._userIpInfo, this._ipAddress);
      localStorage.setItem(
        this._userAccessPermission,
        JSON.stringify(identityViewModel.userModel!.userAccessMappingDetails)
      );
    }
  }

  // Remove tokens
  private removeTokens(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this._jwtToken);
      localStorage.removeItem(this._loginUserInfo);
      localStorage.removeItem(this._userIpInfo);
    }
  }

  // Get jwt token
  getJwtToken(): string {
    if (!this._cachedJwtToken) {
      if (isPlatformBrowser(this.platformId)) {
        this._cachedJwtToken = localStorage.getItem(this._jwtToken);
      }
    }

    return this._cachedJwtToken || "No Token Found!";
  }
}
