import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent {
  isCollapsed: boolean = false;
  constructor(private router: Router) {}

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('user'); // ✅ Check if user is logged in
  }

  logout(): void {
    localStorage.removeItem('user'); // ✅ Remove user session
    this.router.navigate(['/login']); // ✅ Redirect to login page
  }
}
