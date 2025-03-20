import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder, private router: Router, private http: HttpClient) {
    this.loginForm = this.fb.group({
      name: ['', [Validators.required]], // Changed from email to name
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  seePassword() {
    const showpass = document.getElementById("password") as HTMLInputElement;
    showpass.type = showpass.type === "password" ? "text" : "password";
  }
  

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { name, password } = this.loginForm.value;
      console.log('Attempting login with:', name, password);
  
      this.http.post('http://localhost:5000/login', { name, password }).subscribe({
        next: (response: any) => {
          console.log('Login success:', response);
          localStorage.setItem('user', JSON.stringify(response.user));
  
          // Redirect based on user ID
          if (response.user.id === 1 || response.user.id === 2) {
            this.router.navigate(['/home']);
          } else if (response.user.id === 3) {
            this.router.navigate(['/alternate']); // Change this to your actual alternate route
          } else {
            alert('Unexpected user role');
          }
        },
        error: (error) => {
          console.error('Login failed:', error);
          alert('Invalid name or password!');
        }
      });
    }
  }
  
}
