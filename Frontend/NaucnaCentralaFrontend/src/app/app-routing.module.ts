import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RegisterComponent } from '././register/register.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { MagazineCreateComponent } from './magazines/magazine-create/magazine-create.component';
import { AdminTaskListComponent } from './admin-tasks/admin-task-list/admin-task-list.component';
import { EditorTaskListComponent } from './editor-tasks/editor-task-list/editor-task-list.component';
import { AddReviewersComponent } from './editor-tasks/add-reviewers/add-reviewers.component';
import { CorrectMagazineComponent } from './editor-tasks/correct-magazine/correct-magazine.component';

const appRoutes: Routes = [
    { path: '', redirectTo:'pocetna', pathMatch:'full'},
    { path: 'pocetna', component: HomeComponent },
    { path: 'registruj-se', component: RegisterComponent },
    { path: 'uloguj-se', component: LoginComponent},
    { path: 'kreiraj-casopis', component: MagazineCreateComponent},
    { path: 'moji-zadaci', component: AdminTaskListComponent},
    { path: 'zadaci', component: EditorTaskListComponent},
    { path: 'dodaj-recenzente/:id', component: AddReviewersComponent },
    { path: 'koriguj-casopis/:id', component: CorrectMagazineComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(appRoutes)],
    exports: [RouterModule]
})
export class AppRoutingModule {

}