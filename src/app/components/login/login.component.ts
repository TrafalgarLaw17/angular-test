import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  errorMessage: any;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { username, email } = this.loginForm.value;

      this.authService.login(username, email).subscribe({
        next: (response) => {
          console.log("Login successful", response);
          console.log(username, email);
          this.authService.setToken(response.token);
          this.router.navigate(['/home']); // Redirect to home after login
        },
        error: (err) => {
          console.error("Login failed", err);
          console.log(username, email);
          this.errorMessage = "Invalid username or email"
        }
      });
    }
  }
}
