import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5000/list';  // API URL

  constructor(private http: HttpClient) {}

  // Get all data
  getData(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  // Add new data
  addData(name: string, age: number): Observable<any> {
    return this.http.put<any>(this.apiUrl, { name, age });
  }

  // Update existing data
  updateData(id: number, name: string, age: number): Observable<any> {
    return this.http.post<any>(this.apiUrl, { id, name, age });
  }

  // Delete data
  deleteData(id: number): Observable<any> {
    return this.http.delete<any>(this.apiUrl, { body: { id } });
  }
}
