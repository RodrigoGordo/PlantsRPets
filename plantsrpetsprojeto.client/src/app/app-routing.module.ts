import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPageComponent } from './landing-page/landing-page.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { FeaturesComponent } from './features/features.component';
import { FaqComponent } from './faq/faq.component';
import { ContactUsComponent } from './contact-us/contact-us.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SigninComponent } from './signin/signin.component'
import { RegisterComponent } from './register/register.component';
import { PlantationsComponent } from './plantations/plantations.component';
import { CollectionComponent } from './collection/collection.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SettingsComponent } from './settings/settings.component';
import { ProfileComponent } from './profile/profile.component';
import { HomeComponent } from './home/home.component';
import { WikiComponent } from './wiki/wiki.component';
import { PlantInfoPageComponent } from './plant-info-page/plant-info-page.component';
import { PlantationDetailsComponent } from './plantation-details/plantation-details.component';
import { PetDetailsComponent } from './pet-details/pet-details.component';
import { PlantationPlantDetailsComponent } from './plantation-plant-details/plantation-plant-details.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { EmailVerificationSentComponent } from './email-verification-sent/email-verification-sent.component';


const routes: Routes = [
  { path: '', component: LandingPageComponent },
  { path: 'about-us', component: AboutUsComponent },
  { path: 'features', component: FeaturesComponent },
  { path: 'faq', component: FaqComponent },
  { path: 'contact-us', component: ContactUsComponent},
  { path: 'forgot-password', component: ForgotPasswordComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'signin', component: SigninComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'confirm-email', component: ConfirmEmailComponent },
  { path: 'email-verification-sent', component: EmailVerificationSentComponent },
  { path: 'plantations', component: PlantationsComponent },
  { path: 'collections', component: CollectionComponent },
  { path: 'plantation/:id', component: PlantationDetailsComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'home', component: HomeComponent },
  { path: 'wiki', component: WikiComponent },
  { path: 'plant-information/:id', component: PlantInfoPageComponent },
  { path: 'plants/:id', component: PlantInfoPageComponent },
  { path: 'pet/:id', component: PetDetailsComponent },
  { path: 'plantation/:plantationId/plant/:plantInfoId', component: PlantationPlantDetailsComponent },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
