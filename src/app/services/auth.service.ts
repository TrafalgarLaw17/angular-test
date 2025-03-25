import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:7095/api/Account/list'; // Change this if your backend uses a different port

  constructor(private http: HttpClient) {}

  // ✅ Login request (calls .NET API)
  login(username: string, email: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa(username + ':' + email) // Encode username & email
    });
  
    return this.http.get('http://localhost:7095/api/Account/list', { headers });
  }
  

  // ✅ Store JWT token after successful login
  setToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  // ✅ Get JWT token for authentication
  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  getAccounts(): Observable<any> {
    const token = localStorage.getItem('token'); // Get JWT token from storage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  
    return this.http.get('http://localhost:7095/api/Account/list', { headers });
  }
  
}
