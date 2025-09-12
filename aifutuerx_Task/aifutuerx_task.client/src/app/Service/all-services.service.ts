import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';

export interface Product {
  name: { en: string; ar: string };
  description: { en: string; ar: string };
  isActive: string;
  amount: number;
  currency: string;
}

@Injectable({
  providedIn: 'root'
})
export class AllServicesService {

  // ================================
  // API Endpoints
  // ================================
  private apiUrl_AddUser = 'https://localhost:7144/api/User/register';
  private apiUrl_Login = 'https://localhost:7144/api/User/login';
  private apiUrl_ResetPass = 'https://localhost:7144/api/User/reset-password';
  private apiUrl_sendEmail = 'https://localhost:7144/api/User/forgot-password';

  constructor(private http: HttpClient) { }

  // ================================
  // Register
  // ================================
  registerCustomer(customerData: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.apiUrl_AddUser, customerData, { headers })
      .pipe(catchError(this.handleError));
  }

  // ================================
  // Login
  // ================================
  login(email: string, password: string): Observable<string> {
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    return this.http.post<{ token: string }>(this.apiUrl_Login, formData)
      .pipe(
        tap(response => localStorage.setItem('jwtToken', response.token)),
        map(response => response.token),
        catchError(error => throwError(() => new Error(error?.error?.message || 'Login failed')))
      );
  }

  // ================================
  // Token Management
  // ================================
  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
  }

  // ================================
  // Forgot Password
  // ================================
  sendResetPasswordEmail(email: string): Observable<any> {
    return this.http.post(this.apiUrl_sendEmail, { email })
      .pipe(catchError(this.handleError));
  }


  // ================================
  // Verify Reset Token
  // ================================
  verifyResetToken(token: string): Observable<any> {
    return this.http.post(`${this.apiUrl_ResetPass}/verify-reset-token`, { token })
      .pipe(catchError(this.handleError));
  }

  // ================================
  // Reset Password
  // ================================
  resetPassword(email: string, otp: string, newPassword: string): Observable<any> {
    const payload = { email, otp, newPassword };
    console.log('Sending reset payload:', payload); 
    return this.http.post(this.apiUrl_ResetPass, payload)
      .pipe(catchError(this.handleError));
  }



  // ================================
  // Error Handling
  // ================================
  private handleError(error: any) {
    console.error('API Error:', error);
    return throwError(() => new Error(error?.error?.message || 'An error occurred. Please try again.'));
  }
}
