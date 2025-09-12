import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AllServicesService } from '../Service/all-services.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetForm!: FormGroup;
  token: string = '';
  loading: boolean = false;
  successMessage: string = '';
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AllServicesService
  ) { }

  ngOnInit(): void {
    this.resetForm = this.fb.group({
      code: ['', [Validators.required]],         
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });

    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
    });
  }


  passwordMatchValidator(form: AbstractControl): ValidationErrors | null {
    const password = form.get('password')?.value;
    const confirm = form.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  submit() {
  
    console.log('Form Valid?', this.resetForm.valid);
    console.log('Form', this.resetForm);
    if (this.resetForm.invalid) {
      const controls = this.resetForm.controls;

      if (controls['code']?.invalid) this.errorMessage = 'OTP code is required';
      else if (controls['password']?.invalid) this.errorMessage = 'Password must be at least 6 characters';
      else if (controls['confirmPassword']?.invalid) this.errorMessage = 'Please confirm your password';
      else if (this.resetForm.errors && this.resetForm.errors['mismatch']) this.errorMessage = 'Passwords do not match';

      return;
    }

    this.errorMessage = '';
    this.loading = true;
    this.successMessage = '';

    const code = this.resetForm.value.code;
    const password = this.resetForm.value.password;
    const email = localStorage.getItem('resetEmail');

    if (!email) {
      this.errorMessage = 'Email not found. Please start reset process again.';
      this.loading = false;
      return;
    }

    this.authService.resetPassword(email, code, password).subscribe({
      next: () => {
        this.successMessage = 'Password reset successfully! Redirecting to login...';
        this.loading = false;
        localStorage.removeItem('resetEmail');
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        console.error('Reset Password error:', err);
        this.errorMessage = err.error?.message || 'Something went wrong!';
        this.loading = false;
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/Login']);
  }
}
