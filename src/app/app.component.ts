import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  newName: string = '';
  newAge: number | null = null; // Allow null as a valid value
  updateId: number | null = null; // Allow null as a valid value
  updateName: string = '';
  updateAge: number | null = null; // Allow null as a valid value
  deleteId: number | null = null; // Allow null as a valid value
  data: any[] = [];

  private apiUrl = 'http://localhost:5000/list'; // Your Node.js API endpoint

  isLoginPage: boolean = false; // Flag to control header/sidebar visibility

  constructor(private http: HttpClient, private router: Router) {
    this.getData(); // Load data on init

    // Check the current route to hide/show header & sidebar
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.isLoginPage = event.url === '/login';
      }
    });
  }

  // Fetch Data from API
  getData() {
    this.http.get<any>(this.apiUrl).subscribe(
      response => {
        if (response.success) {
          this.data = response.data;
        }
      },
      error => {
        console.error('Error fetching data:', error);
      }
    );
  }

  // Add Data to API (using POST)
  addData() {
    if (this.newName && this.newAge !== null) {
      const newData = { name: this.newName, age: this.newAge };
      this.http.post<any>(this.apiUrl, newData).subscribe(
        response => {
          if (response.success) {
            this.getData(); // Refresh data
            this.newName = '';
            this.newAge = null; // Reset the value to null
          }
        },
        error => {
          console.error('Error adding data:', error);
        }
      );
    }
  }

  // Update Data (using PUT)
  updateData() {
    if (this.updateId !== null && (this.updateName || this.updateAge !== null)) {
      const updatedData = { id: this.updateId, name: this.updateName, age: this.updateAge };
      this.http.put<any>(this.apiUrl, updatedData).subscribe(
        response => {
          if (response.success) {
            this.getData(); // Refresh data
            this.updateId = null;
            this.updateName = '';
            this.updateAge = null; // Reset the value to null
          }
        },
        error => {
          console.error('Error updating data:', error);
        }
      );
    }
  }

  // Delete Data (using DELETE)
  deleteData() {
    if (this.deleteId !== null) {
      this.http.delete<any>(`${this.apiUrl}/${this.deleteId}`).subscribe(
        response => {
          if (response.success) {
            this.getData(); // Refresh data
            this.deleteId = null; // Reset the value to null
          }
        },
        error => {
          console.error('Error deleting data:', error);
        }
      );
    }
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('user');
  }

  logout(): void {
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }
}
