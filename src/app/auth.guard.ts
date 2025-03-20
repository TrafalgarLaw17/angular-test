import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const router = inject(Router); // Inject the Router service
  const isEditable = route.queryParams['isEditable'] === 'true'; // Compare as a string

  if (!isEditable) {
    router.navigate(['/not-editable']);
    return false;
  }
  return true;
};
