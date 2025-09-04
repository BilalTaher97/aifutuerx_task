import { Component } from '@angular/core';
import { AllServicesService } from '../Service/all-services.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']  
})
export class LoginComponent {

  constructor(private service: AllServicesService, private router: Router) { }


  language: 'en' | 'ar' = 'en';

 
  loginModel = {
    email: '',
    password: '',
  };

 
  onSubmit() {
    if (this.loginModel.email && this.loginModel.password) {
      this.service.login(this.loginModel.email, this.loginModel.password)
        .subscribe({
          next: (token) => {
            console.log('Login successful, token:', token);

            if (this.loginModel.email == "Admin@gmail.com") {

              this.router.navigate(['/admin/customers']);
            } else {

              this.router.navigate(['/products']);
            }

            
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
}
