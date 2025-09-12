import { Component } from '@angular/core';
import { AllServicesService } from '../Service/all-services.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginModel = {
    email: '',
    password: '',
  };

  showPassword = false; 

  constructor(private service: AllServicesService, private router: Router) { }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.loginModel.email && this.loginModel.password) {
      this.service.login(this.loginModel.email, this.loginModel.password)
        .subscribe({
          next: (token) => {
            console.log('Login successful, token:', token);
           

            localStorage.setItem('token', token);

            this.router.navigate(['/tasks']);
          },
          error: (err) => {
            console.error('Login failed', err);
            alert('Login failed');
          }
        });
    } else {
      alert('Please Fill All Fields');
    }
  }


  onForgotPassword() {

    this.router.navigate(['/forgotPassord']);
  }
}
