import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { throwError } from 'rxjs';

export interface Product {
  name: { en: string; ar: string; };
  description: { en: string; ar: string; };
  isActive: string;
  amount: number;
  currency: string;
}


@Injectable({
  providedIn: 'root'
})
export class AllServicesService {

  private apiUrl_AddUser = 'https://localhost:7152/api/Customer/register';
  private apiUrl_Login = 'https://localhost:7152/api/Customer/login';



  constructor(private http: HttpClient) { }

  registerCustomer(customerData: any): Observable<any> {
    return this.http.post(this.apiUrl_AddUser, customerData)
      .pipe(
        catchError(this.handleError)
      );
  }

  login(email: string, password: string): Observable<string> {
    const formData = new FormData();
    formData.append('email', email);
    formData.append('password', password);

    return this.http.post<{ token: string }>(this.apiUrl_Login, formData)
      .pipe(
        tap(response => {
          localStorage.setItem('jwtToken', response.token);
        }),
        map(response => response.token),
        catchError(error => {
          return throwError(() => new Error('Login failed'));
        })
      );
  }


  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
  }

  private handleError(error: any) {
    console.error('API error:', error);
    return throwError(() => new Error('حدث خطأ في الاتصال بالخادم'));
  }









}
