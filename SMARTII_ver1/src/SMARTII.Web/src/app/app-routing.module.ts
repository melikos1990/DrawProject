import { NgModule } from '@angular/core';

import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { SystemLoginComponent } from './auth/system-login/system-login.component';
import { AppComponent } from './app.component';
import { SelectivePreloadingStrategy } from './shared/router/selectivePreloading.strategy';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'

  },
  {
    path: 'login',
    component: SystemLoginComponent
  },
  {
    path: 'pages',
    loadChildren: './pages/pages.module#PagesModule',
    data: {
      preload: true
    }
  },
  {
    path: '**',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { preloadingStrategy: SelectivePreloadingStrategy })], // , { preloadingStrategy: PreloadAllModules }
  exports: [RouterModule],
  providers: [SelectivePreloadingStrategy]
})
export class AppRoutingModule { }
