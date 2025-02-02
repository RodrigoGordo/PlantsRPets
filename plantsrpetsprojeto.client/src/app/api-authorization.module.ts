import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { SigninComponent } from './signin/signin.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './register/register.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,    
    ReactiveFormsModule,
    RouterModule.forChild(
      [
        { path: 'signin', component: SigninComponent },
        { path: 'new', component: RegisterComponent },
      ]
    )
  ],
  declarations: [LoginMenuComponent, SigninComponent, RegisterComponent],
  exports: [LoginMenuComponent, SigninComponent, RegisterComponent]
})
export class ApiAuthorizationModule { }
