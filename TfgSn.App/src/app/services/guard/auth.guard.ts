import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';
import { map, catchError } from 'rxjs/operators';
import { AutheticationService } from '../authetication.service';
import { of } from 'rxjs';

export const AuthGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const authenticationService = inject(AutheticationService);
  const router = inject(Router);

  const token = authService.getToken();

  if (!token) {
    router.navigate(['']);
    return false;
  }

  return authenticationService.validateToken(token).pipe(
    map(response => {
      if (response.result) {
        return true;
      } else {
        router.navigate(['']);
        return false;
      }
    }),
    catchError(error => {
      router.navigate(['']);
      return of(false);
    })
  );
};