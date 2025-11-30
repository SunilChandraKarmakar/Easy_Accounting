import { Routes } from '@angular/router';
import { CountriesComponent } from './components/layout/global/countries/countries.component';
import { LayoutComponent } from './components/layout/layout.component';
import { CitiesComponent } from './components/layout/global/cities/cities.component';

export const routes: Routes = [

  // Login component
  { path: "", component: LayoutComponent, pathMatch: "full" },

  // For layout
  {
    path: "app",
    component: LayoutComponent,
    children: [
      { path: "countries", component: CountriesComponent, pathMatch: "full" },
      { path: "cities", component: CitiesComponent, pathMatch: "full" },
    ]
  }
];