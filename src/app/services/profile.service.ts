import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private apiUrl = 'http://localhost:5000/list';

  constructor(private http: HttpClient) {}

  getProfiles(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  createProfile(profile: { name: string; age: number }): Observable<any> {
    return this.http.put(this.apiUrl, profile);
  }

  updateProfile(profile: { id: number; name: string; age: number }): Observable<any> {
    return this.http.post(this.apiUrl, profile);
  }

  deleteProfile(id: number): Observable<any> {
    return this.http.delete(this.apiUrl, { body: { id } });
  }
}
