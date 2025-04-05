import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './authorize.interceptor';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { FeaturesComponent } from './features/features.component';
import { FaqComponent } from './faq/faq.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { FormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginMenuComponent } from './login-menu/login-menu.component';
import { RegisterComponent } from './register/register.component';
import { SigninComponent } from './signin/signin.component';
import { ConfirmDialogueComponent } from './confirm-dialogue/confirm-dialogue.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ReactiveFormsModule } from '@angular/forms';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { PlantationsComponent } from './plantations/plantations.component';
import { CollectionComponent } from './collection/collection.component';
import { CommunitiesComponent } from './communities/communities.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SettingsComponent } from './settings/settings.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { LoadingIndicatorComponent } from './loading-indicator/loading-indicator.component';
import { LogoutConfirmationComponent } from './logout-confirmation/logout-confirmation.component';
import { WeatherComponent } from './weather/weather.component';
import { PlantationCardComponent } from './plantation-card/plantation-card.component';
import { CreatePlantationComponent } from './create-plantation/create-plantation.component';
import { WikiComponent } from './wiki/wiki.component';
import { PlantCardComponent } from './plant-card/plant-card.component';
import { TipsCardComponent } from './tips-card/tips-card.component';
import { CommonModule } from '@angular/common';
import { PlantInfoPageComponent } from './plant-info-page/plant-info-page.component';
import { SunlightFormatPipe } from './sunlight-format.pipe';
import { PlantationDetailsComponent } from './plantation-details/plantation-details.component';
import { AddPlantComponent } from './add-plant/add-plant.component';
import { PlantFilterComponent } from './plant-filter/plant-filter.component';
import { PetDetailsComponent } from './pet-details/pet-details.component';
import { PlantationPlantCardComponent } from './plantation-plant-card/plantation-plant-card.component';
import { PlantationPlantDetailsComponent } from './plantation-plant-details/plantation-plant-details.component';
import { FormatDatePipe } from './format-date.pipe';
import { PlantPeriodWarningComponent } from './plant-period-warning/plant-period-warning.component';
import { PetRewardPopupComponent } from './pet-reward-popup/pet-reward-popup.component';
import { HomePlantationCardComponent } from './home-plantation-card/home-plantation-card.component';
import { HomePetCardComponent } from './home-pet-card/home-pet-card.component';
import { EmailVerificationSentComponent } from './email-verification-sent/email-verification-sent.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { RemovePlantPopupComponent } from './remove-plant-popup/remove-plant-popup.component';
import { RemovePlantationPopupComponent } from './remove-plantation-popup/remove-plantation-popup.component';
import { PetCardComponent } from './pet-card/pet-card.component';
import { NotificationCardComponent } from './notification-card/notification-card.component';

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
    ProfileComponent,
    LoadingIndicatorComponent,
    LogoutConfirmationComponent,
    WeatherComponent,
    PlantationCardComponent,
    CreatePlantationComponent,
    WikiComponent,
    PlantCardComponent,
    PlantInfoPageComponent,
    TipsCardComponent,
    SunlightFormatPipe,
    PlantationDetailsComponent,
    AddPlantComponent,
    PlantFilterComponent,
    PetDetailsComponent,
    PlantationPlantCardComponent,
    PlantationPlantDetailsComponent,
    FormatDatePipe,
    PlantPeriodWarningComponent,
    PetRewardPopupComponent,
    HomePlantationCardComponent,
    HomePetCardComponent,
    EmailVerificationSentComponent,
    ConfirmEmailComponent,
    RemovePlantPopupComponent,
    RemovePlantationPopupComponent,
    PetCardComponent,
    NotificationCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule,
    MatTooltipModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatAutocompleteModule
  ],
  providers: [
    provideAnimationsAsync(),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
