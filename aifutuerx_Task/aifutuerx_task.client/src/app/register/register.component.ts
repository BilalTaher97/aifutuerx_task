import { Component, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AllServicesService } from '../Service/all-services.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  customerForm!: FormGroup;
  isArabic: boolean = true;
  photoPreview: string | ArrayBuffer | null = null;
  isLoading: boolean = false;
  showPassword = false;
  @ViewChild('photoInput') photoInput!: ElementRef;

  constructor(private fb: FormBuilder,
    private customerService: AllServicesService) {
    this.createForm();
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }


  createForm() {
    this.customerForm = this.fb.group({
      Name: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Phone: ['', [Validators.required, Validators.pattern('[0-9]{10}')]],
      DateOfBirth: ['', Validators.required],
      Gender: ['', Validators.required],
      Password: ['', Validators.required],
    });
  }




  onSubmit() {
    if (this.customerForm.valid) {
      this.isLoading = true;

      const payload = {
        name: this.customerForm.get('Name')?.value,
        email: this.customerForm.get('Email')?.value,
        phone: this.customerForm.get('Phone')?.value,
        dateOfBirth: this.customerForm.get('DateOfBirth')?.value,
        gender: this.customerForm.get('Gender')?.value,
        password: this.customerForm.get('Password')?.value
      };

      console.log("Payload being sent:", payload); 

      this.customerService.registerCustomer(payload).subscribe(
        (response) => {
          this.isLoading = false;
          alert('Registration successful!');
          this.customerForm.reset();
        },
        (error) => {
          this.isLoading = false;
          alert('Registration failed');
          console.error('Error:', error);
        }
      );
    }
  }


}
