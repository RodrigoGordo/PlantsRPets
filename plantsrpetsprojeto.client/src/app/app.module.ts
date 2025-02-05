import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { FeaturesComponent } from './features/features.component';
import { FaqComponent } from './faq/faq.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { FormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { RegisterComponent } from './register/register.component';
import { SigninComponent } from './signin/signin.component';
import { ConfirmDialogueComponent } from './confirm-dialogue/confirm-dialogue.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { PlantationsComponent } from './plantations/plantations.component';
import { CollectionComponent } from './collection/collection.component';
import { CommunitiesComponent } from './communities/communities.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SettingsComponent } from './settings/settings.component';
import { HomeComponent } from './home/home.component';


@NgModule({
  declarations: [
    AppComponent,
    LandingPageComponent,
    AboutUsComponent,
    FeaturesComponent,
    FaqComponent,
    ContactUsComponent,
    ResetPasswordComponent,
    ForgotPasswordComponent,
    NavbarComponent,
    LoginMenuComponent,
    RegisterComponent,
    SigninComponent,
    ConfirmDialogueComponent,
    PlantationsComponent,
    CollectionComponent,
    CommunitiesComponent,
    DashboardComponent,
    SettingsComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MatDialogModule, MatFormFieldModule,
    MatInputModule, MatButtonModule
  ],
  providers: [
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
