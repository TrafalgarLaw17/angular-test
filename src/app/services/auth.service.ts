import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Todo } from '../../app/components/todo-list/todo.model';
import { TodoTaskStatus } from '../components/todo-list/todo-task-status.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:7095/api/Account/list'; 
  private apiUrl2 = 'http://localhost:7095/api/Todo';
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('token'); // Adjust based on your storage method
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  constructor(private http: HttpClient) {}

  // ✅ Login request (calls .NET API)
  login(username: string, email: string): Observable<any> {
    const headers = new HttpHeaders({
      'Authorization': 'Basic ' + btoa(username + ':' + email) // Encode username & email
    });
  
    return this.http.get(this.apiUrl, { headers });
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

  getTodos(): Observable<Todo[]> {
    return this.http.get<{ data: Todo[] }>(this.apiUrl2).pipe(
      map(response => response.data || []) // Ensure it always returns an array
    );
  }
  

  getTodoTaskStatuses(): Observable<TodoTaskStatus[]> {
    return this.http.get<{ data: TodoTaskStatus[] }>('http://localhost:7095/api/TodoTaskStatus/task-status-list').pipe(
      map(response => response.data || []) // Ensure it's always an array
    );
  }
  
  
  
}