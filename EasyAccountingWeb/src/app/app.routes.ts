import { Routes } from '@angular/router';
import { CountriesComponent } from './components/layout/global/countries/countries.component';
import { LayoutComponent } from './components/layout/layout.component';
import { CitiesComponent } from './components/layout/global/cities/cities.component';
import { CountryCreateComponent } from './components/layout/global/countries/country-create/country-create.component';
import { CountryUpdateComponent } from './components/layout/global/countries/country-update/country-update.component';
import { CityCreateComponent } from './components/layout/global/cities/city-create/city-create.component';
import { CityUpdateComponent } from './components/layout/global/cities/city-update/city-update.component';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { AccessDeniedComponent } from './shared/access-denied/access-denied.component';
import { AuthGuard } from './identity-shared/auth.guard';
import { LoginComponent } from './authentication/login/login.component';
import { CurrenciesComponent } from './components/master-settings/currencies/currencies.component';
import { CurrencyCreateComponent } from './components/master-settings/currencies/currency-create/currency-create.component';
import { CurrencyUpdateComponent } from './components/master-settings/currencies/currency-update/currency-update.component';
import { CompaniesComponent } from './components/master-settings/companies/companies.component';
import { CompanyCreateComponent } from './components/master-settings/companies/company-create/company-create.component';
import { CompanyUpdateComponent } from './components/master-settings/companies/company-update/company-update.component';
import { InvoiceSettingsComponent } from './components/master-settings/Invoice-settings/Invoice-settings.component';
import { InvoiceSettingCreateComponent } from './components/master-settings/Invoice-settings/Invoice-setting-create/Invoice-setting-create.component';
import { InvoiceSettingUpdateComponent } from './components/master-settings/Invoice-settings/Invoice-setting-update/Invoice-setting-update.component';
import { ModulesComponent } from './components/master-settings/modules/modules.component';
import { ModuleCreateComponent } from './components/master-settings/modules/module-create/module-create.component';
import { ModuleUpdateComponent } from './components/master-settings/modules/module-update/module-update.component';
import { ActionsComponent } from './components/master-settings/AccessControl/actions/actions.component';
import { ActionCreateComponent } from './components/master-settings/AccessControl/actions/action-create/action-create.component';
import { ActionUpdateComponent } from './components/master-settings/AccessControl/actions/action-update/action-update.component';
import { FeaturesComponent } from './components/master-settings/AccessControl/features/features.component';
import { FeatureCreateComponent } from './components/master-settings/AccessControl/features/feature-create/feature-create.component';
import { FeatureUpdateComponent } from './components/master-settings/AccessControl/features/feature-update/feature-update.component';
import { FeatureActionsComponent } from './components/master-settings/AccessControl/feature-actions/feature-actions.component';
import { FeatureActionCreateComponent } from './components/master-settings/AccessControl/feature-actions/feature-action-create/feature-action-create.component';
import { FeatureActionUpdateComponent } from './components/master-settings/AccessControl/feature-actions/feature-action-update/feature-action-update.component';
import { EmployeeFeatureActionsComponent } from './components/master-settings/AccessControl/employee-feature-actions/employee-feature-actions.component';
import { CreateEmployeeFeatureActionComponent } from './components/master-settings/AccessControl/employee-feature-actions/create-employee-feature-action/create-employee-feature-action.component';
import { UpdateEmployeeFeatureActionComponent } from './components/master-settings/AccessControl/employee-feature-actions/update-employee-feature-action/update-employee-feature-action.component';
import { CreateVatTaxComponent } from './components/master-settings/vat-taxes/create-vat-tax/create-vat-tax.component';
import { UpdateVatTaxComponent } from './components/master-settings/vat-taxes/update-vat-tax/update-vat-tax.component';
import { VatTaxesComponent } from './components/master-settings/vat-taxes/vat-taxes.component';

export const routes: Routes = [

  // Login component
  { path: "", component: LoginComponent },
  { path: "registration", component: RegistrationComponent, pathMatch: "full" },
  { path: "login", component: LoginComponent, pathMatch: "full" },

  // For layout
  {
    path: "app",
    component: LayoutComponent,
    children: [
      
      // Country
      { path: "countries", component: CountriesComponent, pathMatch: "full" },
      { path: "country/create", component: CountryCreateComponent, pathMatch: "full" },
      { path: "country/update/:recordId", component: CountryUpdateComponent, pathMatch: "full" },

      // City
      { path: "cities", component: CitiesComponent, pathMatch: "full" },
      { path: "city/create", component: CityCreateComponent, pathMatch: "full" },
      { path: "city/update/:recordId", component: CityUpdateComponent, pathMatch: "full" },

      // Currency
      { path: "currencies", component: CurrenciesComponent, pathMatch: "full" },
      { path: "currency/create", component: CurrencyCreateComponent, pathMatch: "full" },
      { path: "currency/update/:recordId", component: CurrencyUpdateComponent, pathMatch: "full" },

      // Company
      { path: "companies", component: CompaniesComponent, pathMatch: "full" },
      { path: "company/create", component: CompanyCreateComponent, pathMatch: "full" },
      { path: "company/update/:recordId", component: CompanyUpdateComponent, pathMatch: "full" },

      // Invoice setting
      { path: "invoice-settings", component: InvoiceSettingsComponent, pathMatch: "full" },
      { path: "invoice-setting/create", component: InvoiceSettingCreateComponent, pathMatch: "full" },
      { path: "invoice-setting/update/:recordId", component: InvoiceSettingUpdateComponent, pathMatch: "full" },

      // Module
      { path: "modules", component: ModulesComponent, pathMatch: "full" },
      { path: "module/create", component: ModuleCreateComponent, pathMatch: "full" },
      { path: "module/update/:recordId", component: ModuleUpdateComponent, pathMatch: "full" },

      // Action
      { path: "actions", component: ActionsComponent, pathMatch: "full" },
      { path: "action/create", component: ActionCreateComponent, pathMatch: "full" },
      { path: "action/update/:recordId", component: ActionUpdateComponent, pathMatch: "full" },

      // Feature
      { path: "features", component: FeaturesComponent, pathMatch: "full" },
      { path: "feature/create", component: FeatureCreateComponent, pathMatch: "full" },
      { path: "feature/update/:recordId", component: FeatureUpdateComponent, pathMatch: "full" },

      // Feature Actions
      { path: "feature-actions", component: FeatureActionsComponent, pathMatch: "full" },
      { path: "feature-action/create", component: FeatureActionCreateComponent, pathMatch: "full" },
      { path: "feature-action/update/:recordId", component: FeatureActionUpdateComponent, pathMatch: "full" },

      // Employee Feature Actions
      { path: "employee-feature-actions", component: EmployeeFeatureActionsComponent, pathMatch: "full" },
      { path: "employee-feature-action/create", component: CreateEmployeeFeatureActionComponent, pathMatch: "full" },
      { path: "employee-feature-action/update/:recordId", component: UpdateEmployeeFeatureActionComponent, pathMatch: "full" },

      // Vat Taxes
      { path: "vat-taxes", component: VatTaxesComponent, pathMatch: "full" },
      { path: "vat-tax/create", component: CreateVatTaxComponent, pathMatch: "full" },
      { path: "vat-tax/update/:recordId", component: UpdateVatTaxComponent, pathMatch: "full" }
    ],
    
    canActivate: [AuthGuard] 
  },
  
  { path: "**", component: AccessDeniedComponent, pathMatch: "full" }, 
];