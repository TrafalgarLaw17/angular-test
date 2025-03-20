import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-view-profiles',
  templateUrl: './view-profiles.component.html',
  styleUrls: ['./view-profiles.component.css'],
})
export class ViewProfilesComponent implements OnInit {
  allProfiles: any[] = []; // Store original unfiltered profiles
  profiles: any[] = []; // Store displayed profiles

  isModalOpen: boolean = false;
  isNamesModalOpen: boolean = false;
  isTotalAgeModalOpen: boolean = false;
  profileToDelete: any = null;
  namesAndAges: string[] = [];
  totalAge: number = 0;
  selectedSort: string = 'name-asc'; // Default sorting option
  selectedFilter: string = 'all'; // Default filter option

  constructor(private profileService: ProfileService, private router: Router) {}

  ngOnInit(): void {
    this.loadProfiles();
  }

  loadProfiles(): void {
    this.profileService.getProfiles().subscribe((response) => {
      console.log("Profiles received:", response.data); // Debugging
  
      // Ensure isEditable is always defined
      this.allProfiles = response.data.map((profile: { isEditable: any; }) => ({
        ...profile,
        isEditable: profile.isEditable ?? false
      }));
  
      this.applyFiltersAndSorting();
    });
  }
  

  onSortChange(event: any) {
    this.selectedSort = event.target.value;
    this.applyFiltersAndSorting();
  }

  onFilterChange(event: any) {
    this.selectedFilter = event.target.value;
    this.applyFiltersAndSorting();
  }

  applyFiltersAndSorting() {
    let filteredProfiles = [...this.allProfiles];

    // Apply filtering
    if (this.selectedFilter === 'below-18') {
      filteredProfiles = filteredProfiles.filter(profile => profile.age < 18);
    } else if (this.selectedFilter === 'above-18') {
      filteredProfiles = filteredProfiles.filter(profile => profile.age >= 18);
    }

    // Apply sorting
    switch (this.selectedSort) {
      case 'name-asc':
        filteredProfiles.sort((a, b) => a.name.localeCompare(b.name));
        break;
      case 'name-desc':
        filteredProfiles.sort((a, b) => b.name.localeCompare(a.name));
        break;
      case 'age-asc':
        filteredProfiles.sort((a, b) => a.age - b.age);
        break;
      case 'age-desc':
        filteredProfiles.sort((a, b) => b.age - a.age);
        break;
    }

    this.profiles = filteredProfiles;
    this.updateNamesAndAges();
  }

  updateNamesAndAges() {
    this.namesAndAges = this.profiles.map(profile => `${profile.name} - ${profile.age} years old`);
  }

  calculateAverageAge() {
    if (this.profiles.length === 0) {
      this.totalAge = 0;
    } else {
      const total = this.profiles.reduce((sum, profile) => sum + profile.age, 0);
      this.totalAge = total / this.profiles.length;
    }
    this.isTotalAgeModalOpen = true;
  }
  
  closeTotalAgeModal() {
    this.isTotalAgeModalOpen = false;
  }

  openNamesModal() {
    this.isNamesModalOpen = true;
  }

  closeNamesModal() {
    this.isNamesModalOpen = false;
  }

  closeModal() {
    this.isModalOpen = false;
    this.profileToDelete = null;
  }

  deleteProfile(id: number): void {
    this.profileToDelete = this.profiles.find(profile => profile.id === id);
    this.isModalOpen = true;
  }

  confirmDelete(): void {
    if (this.profileToDelete) {
      const id = this.profileToDelete.id;

      this.profileService.deleteProfile(id).subscribe(() => {
        this.allProfiles = this.allProfiles.filter(profile => profile.id !== id);
        this.applyFiltersAndSorting();
        this.isModalOpen = false;
        this.profileToDelete = null;
      }, error => {
        console.error("Error deleting profile:", error);
      });
    }
  }

  navigateToCreate(): void {
    this.router.navigate(['/create']);
  }

  navigateToEdit(id: number): void {
    const profile = this.profiles.find(p => p.id === id);
    
    if (profile !== undefined && profile.isEditable !== undefined) {
      this.router.navigate(['/edit', id], { 
        queryParams: { isEditable: String(profile.isEditable) } 
      });
    } else {
      console.error('Profile not found or isEditable is undefined:', profile);
    }
  }
   
  
}


