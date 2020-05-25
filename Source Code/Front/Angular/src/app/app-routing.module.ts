import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './Views/login/login.component';
import { ShowDataComponent } from './Views/show-data/show-data.component';


const routes: Routes = [
    {
        path: '',
        component: LoginComponent
    }, {
        path: 'show',
        component: ShowDataComponent
    }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
