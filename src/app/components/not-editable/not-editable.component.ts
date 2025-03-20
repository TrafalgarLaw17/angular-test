import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-not-editable',
  templateUrl: './not-editable.component.html',
  styleUrls: ['./not-editable.component.css']
})
export class NotEditableComponent {
  constructor(private router: Router) {}

  goBack(): void {
    this.router.navigate(['/profiles']);
  }
}
