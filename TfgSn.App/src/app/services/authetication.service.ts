import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http'; 
import { Login } from '../models/login';
import { Register } from '../models/register';
import { Observable } from 'rxjs';
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

  public register(userData: any): Observable<string> {
    const fetchPromise = fetch(`${environment.apiUrl}/${this.registerUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(userData) // Utilizar los datos del parámetro
    })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.text();
    })
    .then(data => {
      console.log(data); // Imprimir la respuesta en la consola
      return data;
    })
    .catch(error => {
      console.error(error); // Imprimir el error en la consola
      throw error;
    });

    return new Observable<string>(observer => {
      fetchPromise
      .then(data => {
        observer.next(data);
        observer.complete();
      })
      .catch(error => {
        observer.error(error);
      });
    });
  }



  public login(user: Login): Observable<JwtAuth> {
    return this.http.post<JwtAuth>(`${environment.apiUrl}/${this.loginUrl}`, user)
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
