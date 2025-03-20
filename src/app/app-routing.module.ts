import { NgModule } from '@angular/core';
import { Router, RouterModule, Routes } from '@angular/router';
import { ViewProfilesComponent } from './components/view-profiles/view-profiles.component';
import { CreateProfileComponent } from './components/create-profile/create-profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { HomeComponent } from './components/home/home.component'; 
import { NotEditableComponent } from './components/not-editable/not-editable.component'; // ✅ Fixed import path
import { LoginComponent } from './components/login/login.component';
import { authGuard } from './auth.guard';
import { PermissionsComponent } from './components/permissions/permissions.component';



const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent },
  { path: 'profiles', component: ViewProfilesComponent },
  { path: 'create', component: CreateProfileComponent },
  { path: 'edit/:id', component: EditProfileComponent, canActivate: [authGuard] }, // ✅ Guard applied
  { path: 'not-editable', component: NotEditableComponent }, 
  { path: 'permissions', component: PermissionsComponent }, 
  { path: '**', redirectTo: 'login' } // Redirect to login if unknown route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule {}

