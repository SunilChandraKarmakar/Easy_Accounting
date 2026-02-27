import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { DefaultUrlSerializer, provideRouter, UrlSerializer, UrlTree, withComponentInputBinding } from '@angular/router';
import { provideToastr } from "ngx-toastr";
import { routes } from './app.routes';
import { provideAnimations } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import { API_BASE_URL, AuthenticationService } from '../api/base-api';
import { environment } from '../environments/environment.prod';
import { provideNzIcons } from 'ng-zorro-antd/icon';
import { FastBackwardOutline, PlusCircleOutline, PlusOutline, SaveOutline, CloseOutline, BankOutline, HomeOutline, TagOutline, AimOutline, FormOutline, DeleteOutline, DollarOutline, PayCircleOutline, InfoOutline, MailOutline, PhoneOutline, BorderlessTableOutline, FireOutline, BranchesOutline, NumberOutline, TableOutline, ProfileOutline, AppstoreOutline, UserOutline, ContainerOutline, UnderlineOutline, UnorderedListOutline } from '@ant-design/icons-angular/icons';
import { IdentityService } from './identity-shared/identity.service';
import { IdentityInterceptor } from './identity-shared/identity-interceptor';
import { NZ_I18N, en_US } from 'ng-zorro-antd/i18n';

export class LowerCaseUrlSerializer extends DefaultUrlSerializer {
  override parse(url: string): UrlTree {
    return super.parse(url.toLowerCase());
  }
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(withFetch(), withInterceptorsFromDi()),
    provideAnimations(),
    provideZoneChangeDetection({ eventCoalescing: true }),

    provideToastr({
      timeOut: 3000, 
      positionClass: "toast-bottom-right", 
      closeButton: true,
      progressBar: true, 
      preventDuplicates: true,
      newestOnTop: true
    }),

    // NG Zorro Icons
    provideNzIcons([
      FastBackwardOutline,
      PlusOutline,
      PlusCircleOutline,
      SaveOutline,
      CloseOutline,
      BankOutline,
      HomeOutline,
      TagOutline,
      AimOutline,
      FormOutline, 
      DeleteOutline,
      DollarOutline,
      PayCircleOutline,
      InfoOutline,
      MailOutline,
      PhoneOutline,
      BorderlessTableOutline,
      FireOutline,
      BranchesOutline,
      NumberOutline,
      TableOutline,
      ProfileOutline,
      AppstoreOutline,
      UserOutline,
      ContainerOutline,
      UnderlineOutline,
      UnorderedListOutline
    ]),

    AuthenticationService,
    IdentityService,

    { provide: NZ_I18N, useValue: en_US },
    { provide: UrlSerializer, useClass: LowerCaseUrlSerializer },
    { provide: HTTP_INTERCEPTORS, useClass: IdentityInterceptor, multi: true },
    { provide: API_BASE_URL, useValue: environment.coreBaseUrl }
  ]
};