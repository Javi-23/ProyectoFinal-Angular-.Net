import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpErrorResponse, HttpResponse } from '@angular/common/http'; 
import { Login } from '../models/login';
import { Register } from '../models/register';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { JwtAuth } from '../models/jwtAuth';

@Injectable({
  providedIn: 'root'
})
export class AutheticationService {
  loginUrl = "Auth/Login"
  validateTokenUrl = "Auth/ValidateToken"
  weatherUrl = "WeatherForecast"
  registerUrl = "Auth/Register"

  constructor(private http: HttpClient) { }

  register(userData: any): Observable<any> {
    return this.http.post(`${environment.apiUrl}/${this.registerUrl}`, userData)
      .pipe(
        catchError(this.handleError)
      );
  }

  login(user: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.loginUrl}`, user)
      .pipe(
        catchError(this.handleLoginError)
      );
  }

  private handleLoginError(error: HttpErrorResponse) {
    let errorMessage = 'Usuario y/o contraseña incorrectos.';
    if (error.error) {
      if (typeof error.error === 'object' && error.error.hasOwnProperty('')) {
        const serverErrors = error.error[''];
        if (Array.isArray(serverErrors) && serverErrors.length > 0) {
          errorMessage = serverErrors.join(', ');
        }
      } else if (error.error.errors) {
        errorMessage = error.error.errors.join(', ');
      } else if (error.error instanceof ErrorEvent) {
        errorMessage = `Error: ${error.error.message}`;
      }
    }
    return throwError(errorMessage);
  }


  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Ha ocurrido un error inesperado.';
    if (error.error) {
      if (typeof error.error === 'object' && error.error.hasOwnProperty('')) {
        const serverErrors = error.error[''];
        if (Array.isArray(serverErrors) && serverErrors.length > 0) {
          errorMessage = serverErrors.join(', ');
        }
      } else if (error.error.errors) {
        errorMessage = error.error.errors.join(', ');
      } else if (error.error instanceof ErrorEvent) {
        errorMessage = `Error: ${error.error.message}`;
      }
    }
    return throwError(errorMessage);
  }

  public validateToken(token: string): Observable<any> {
    const requestBody = { Token : token };
    return this.http.post<any>(`${environment.apiUrl}/${this.validateTokenUrl}`, requestBody);
  }

  public getWeather(): Observable<any> {
    return this.http.get<any>(`${environment.apiUrl}/${this.weatherUrl}`);
  }

  /*
    fetch('https://localhost:7047/api/Auth/Register', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      userName: "UserNoName",
      email: "exampleEssmail@example.com",
      description: "Example description",
      password: "ExamplePassword.12345"
    })
  })
  .then(response => response.text()) // Analizar la respuesta como texto
  .then(data => console.log(data)) // Imprimir la respuesta en la consola
  */
}
