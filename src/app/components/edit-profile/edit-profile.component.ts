import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileService } from 'src/app/services/profile.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent {
  // Properties bound to the template
  id: number | null = null;
  name: string = '';
  age: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private profileService: ProfileService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadProfile();
  }

  loadProfile(): void {
    this.profileService.getProfiles().subscribe((response) => {
      const profile = response.data.find((p: any) => p.id === this.id);
      if (profile) {
        this.name = profile.name;
        this.age = profile.age;
      }
    });
  }

  // Method to update the profile
  updateProfile(): void {
    if (this.name && this.age && this.id !== null) {
      this.profileService.updateProfile({ id: this.id, name: this.name, age: this.age }).subscribe(() => {
        this.router.navigate(['/profiles']);
        console.log('Profile updated:', { name: this.name, age: this.age });
      alert('Profile updated successfully!');
      });
    } else {
      alert('Please fill in all fields.');
    }
  }
}
