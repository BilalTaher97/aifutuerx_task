import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AllServicesService } from '../Service/all-services.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'] 
})
export class ForgotPasswordComponent {
  email: string = '';

  constructor(private authService: AllServicesService, private router: Router) { }

  sendResetLink() {
    if (!this.email) return;

    this.authService.sendResetPasswordEmail(this.email).subscribe({
      next: () => {
        localStorage.setItem('resetEmail', this.email);
        this.router.navigate(['/resetPassword']);
      },
      error: () => alert('Failed to send reset link.')
    });
  }


}
