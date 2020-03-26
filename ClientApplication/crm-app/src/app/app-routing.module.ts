import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthComponent } from './components/auth/auth.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { HomeComponent } from './components/home/home.component';
import { SettingsComponent } from './components/settings/settings.component';
import { DictionaryComponent } from './components/dictionary/dictionary.component';

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
