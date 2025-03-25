import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { MatTableModule } from '@angular/material/table';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';


import { AppComponent } from './app.component';
import { ViewProfilesComponent } from './components/view-profiles/view-profiles.component';
import { CreateProfileComponent } from './components/create-profile/create-profile.component';
import { EditProfileComponent } from './components/edit-profile/edit-profile.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { HomeComponent } from './components/home/home.component';
import { NotEditableComponent } from './components/not-editable/not-editable.component';
import { LoginComponent } from './components/login/login.component';
import { PermissionsComponent } from './components/permissions/permissions.component';


@NgModule({
  declarations: [
    AppComponent,
    ViewProfilesComponent,
    CreateProfileComponent,
    EditProfileComponent,
    SidebarComponent,
    HomeComponent,
    NotEditableComponent,
    LoginComponent,
    PermissionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MatTableModule,
    MatCheckboxModule,
    MatButtonModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
