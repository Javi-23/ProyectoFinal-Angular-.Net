import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  usernameUser = 'User';
  allUsers = 'GetAllUsers'
  userPrefix = 'UsertsByPrefix';
  updateDescritpion = 'update-description';
  uploadImage = 'upload-image';

  constructor(private http : HttpClient) { }

  getUser(): Observable<UserViewModel> {
    const url = `${environment.apiUrl}/${this.usernameUser}`;
    return this.http.get<UserViewModel>(url);
  } 

  getUserByUsername(username: string): Observable<UserViewModel> {
    const url = `${environment.apiUrl}/${this.usernameUser}/${username}`;
    return this.http.get<UserViewModel>(url);
  }

  getAllUsers(): Observable<UserViewModel[]> {
    const url = `${environment.apiUrl}/${this.usernameUser}/${this.allUsers}`;
    return this.http.get<UserViewModel[]>(url);
  }

  getUsersByPrefix(prefix: string): Observable<UserViewModel[]> {
    const url = `${environment.apiUrl}/user/usersByPrefix?prefix=${encodeURIComponent(prefix)}`; 
    return this.http.get<UserViewModel[]>(url);
  }

  updateUserDescription(newDescription: string): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });

    return this.http.put(`${environment.apiUrl}/${this.usernameUser}/${this.updateDescritpion}`, JSON.stringify(newDescription), { headers })
      .pipe(
        catchError(error => {
          console.error('Error actualizando la descripción del usuario', error);
          return throwError(() => new Error(error));
        })
      );
  }

  uploadUserImage(imageFile: File): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    const formData: FormData = new FormData();
    formData.append('imageFile', imageFile);

    return this.http.put(`${environment.apiUrl}/${this.usernameUser}/${this.uploadImage}`, formData, { headers })
      .pipe(
        catchError(error => {
          console.error('Error subiendo la imagen del usuario', error);
          return throwError(() => new Error(error));
        })
      );
  }

  downloadUploadedImage(username: string): Observable<HttpResponse<Blob>> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    const params = new HttpParams().set('username', username);
    const url = `${environment.apiUrl}/${this.usernameUser}/download-uploaded-image`;

    return this.http.get(url, { headers, params, responseType: 'blob', observe: 'response' })
      .pipe(
        catchError(error => {
          console.error('Error descargando la imagen del usuario', error);
          return throwError(() => new Error(error));
        })
      );
  }
}

