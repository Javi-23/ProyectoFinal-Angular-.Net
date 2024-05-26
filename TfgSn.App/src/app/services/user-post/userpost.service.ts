import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class UserpostService {

  userPostUrl = 'UserAndPost/posts/all';
  followedPostUrl = 'UserAndPost/posts/followed';
  usernamePost = 'UserAndPost/posts';

  constructor(private http: HttpClient) { }

  getAllPosts(): Observable<UserAndPostDto[]> {
    return this.http.get<UserAndPostDto[]>(`${environment.apiUrl}/${this.userPostUrl}`);
  }

  getFollowedPost(): Observable<UserAndPostDto[]> {
    return this.http.get<UserAndPostDto[]>(`${environment.apiUrl}/${this.followedPostUrl}`);
  }

  getUserPost(username: string): Observable<UserAndPostDto> {
    const url = `${environment.apiUrl}/${this.usernamePost}/${username}`;
    return this.http.get<UserAndPostDto>(url);
  }  
}