import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home.component';
import { CcHomeComponent } from './home-page/cc-home/cc-home.component';
import { HeaderquaterHomeComponent } from './home-page/headerquater-home/headerquater-home.component';
import { VendorHomeComponent } from './home-page/vendor-home/vendor-home.component';
import { HomePageComponent } from './home-page/home-page.component';
import { AuthGuardService } from 'src/app/shared/service/auth-guard.service';

const routes: Routes = [{
  path: '',
  component: HomeComponent,
  children: [
    {
      path: 'home-page',
      component: HomePageComponent,
    },

  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [
    RouterModule
  ]
})
export class HomeRoutingModule { }
