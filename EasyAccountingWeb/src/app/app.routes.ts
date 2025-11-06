import { Routes } from '@angular/router';
import { CountriesComponent } from './components/layout/global/countries/countries.component';
import { LayoutComponent } from './components/layout/layout.component';

export const routes: Routes = [
    // Login component
  { path: "", component: CountriesComponent, pathMatch: "full" },
  { path: "countries", component: CountriesComponent, pathMatch: "full" },

  // For layout
  {
    path: "app",
    component: LayoutComponent,
    children: [
     
    ]
  }
];