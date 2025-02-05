import { CanActivateFn, Router} from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {

  const router = inject(Router);

  const storedEmail = localStorage.getItem('UserEmail');

  if (storedEmail) {
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};


