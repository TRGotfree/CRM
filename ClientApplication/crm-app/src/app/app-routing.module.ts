import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { HomeComponent } from './home/home.component';
import { SettingsComponent } from './settings/settings.component';
import { DictionaryComponent } from './dictionary/dictionary.component';

const routes: Routes = [
  { path: '', component: AuthComponent },
  { path: 'authentification', component: AuthComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'dictionary', component: DictionaryComponent },
  { path: 'home', component: HomeComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
