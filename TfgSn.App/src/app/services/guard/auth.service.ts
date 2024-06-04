import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }
  IsLoggedIn() {
    return !!localStorage.getItem('jwtToken')
  }

  public getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  public clearToken(): void {
    localStorage.removeItem('jwtToken');
  }
}
