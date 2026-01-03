import { Routes } from '@angular/router';
import { CountriesComponent } from './components/layout/global/countries/countries.component';
import { LayoutComponent } from './components/layout/layout.component';
import { CitiesComponent } from './components/layout/global/cities/cities.component';
import { CountryCreateComponent } from './components/layout/global/countries/country-create/country-create.component';
import { CountryUpdateComponent } from './components/layout/global/countries/country-update/country-update.component';
import { CityCreateComponent } from './components/layout/global/cities/city-create/city-create.component';
import { CityUpdateComponent } from './components/layout/global/cities/city-update/city-update.component';
import { RegistrationComponent } from './authentication/registration/registration.component';

export const routes: Routes = [

  // Login component
  { path: "", component: RegistrationComponent },

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
      { path: "city/update/:recordId", component: CityUpdateComponent, pathMatch: "full" }
    ] 
  }
];