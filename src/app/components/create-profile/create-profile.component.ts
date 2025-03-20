import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-create-profile',
  templateUrl: './create-profile.component.html',
  styleUrls: ['./create-profile.component.css'],
})
export class CreateProfileComponent {
  name: string = '';
  age: number | null = null;

  constructor(private profileService: ProfileService, private router: Router) {}

  createProfile(): void {
    if (this.name && this.age) {
      this.profileService.createProfile({ name: this.name, age: this.age }).subscribe(() => {
        this.router.navigate(['/profiles']);
      });
    } else {
      alert('Please fill in all fields.');
    }
  }
}
