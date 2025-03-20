import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  private apiUrl = 'http://localhost:5000';

  constructor(private http: HttpClient) {}

  // 🟢 Fetch Base Permissions
  getBasePermissions(): Observable<any> {
    return this.http.get(`${this.apiUrl}/base-permissions`);
  }

  // 🟢 Fetch User Permissions
  getUserPermissions(): Observable<any> {
    return this.http.get(`${this.apiUrl}/user-permissions`);
  }

  // 🟢 Update Permissions
  updatePermissions(permissions: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/update-permissions`, { permissions });
  }
}
