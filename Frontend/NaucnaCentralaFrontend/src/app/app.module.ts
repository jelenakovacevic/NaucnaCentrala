import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { RegisterComponent } from './register/register.component';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { MagazineCreateComponent } from './magazines/magazine-create/magazine-create.component';
import { TokenInterceptor } from './authentication/token.interceptor';
import { AdminTaskListComponent } from './admin-tasks/admin-task-list/admin-task-list.component';
import { EditorTaskListComponent } from './editor-tasks/editor-task-list/editor-task-list.component';
import { AddReviewersComponent } from './editor-tasks/add-reviewers/add-reviewers.component';
import { CorrectMagazineComponent } from './editor-tasks/correct-magazine/correct-magazine.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    RegisterComponent,
    LoginComponent,
    HomeComponent,
    MagazineCreateComponent,
    AdminTaskListComponent,
    EditorTaskListComponent,
    AddReviewersComponent,
    CorrectMagazineComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
