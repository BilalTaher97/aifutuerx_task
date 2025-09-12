import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { TasksComponent } from './tasks/tasks.component';
import { CreateTaskComponent } from './create-task/create-task.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './resetpassword/resetpassword.component';

const routes: Routes = [

  { path: '', redirectTo: '/Login', pathMatch: 'full' },
  { path: "Login", component: LoginComponent },
  { path: "register", component: RegisterComponent },
  { path: "tasks", component: TasksComponent },
  { path: "createTask", component: CreateTaskComponent },
  { path: "forgotPassord", component: ForgotPasswordComponent },
  { path: "resetPassword", component: ResetPasswordComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
