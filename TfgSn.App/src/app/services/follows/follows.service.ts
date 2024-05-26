import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, catchError, map, throwError } from 'rxjs';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class FollowsService {
  followedUrl = 'UserToFollow/followed-users';
  followersUrl = 'UserToFollow/follower-users';
  followUrl = 'UserToFollow/follow-user'
  unfollowUrl = 'UserToFollow/unfollow-user'

  constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

  getFollowed(username: string): Observable<UserViewModel[]> {
    const url = `${environment.apiUrl}/${this.followedUrl}/${username}`;
    return this.http.get<UserViewModel[]>(url);
  }

  getFollowers(username: string): Observable<UserViewModel[]> {
    const url = `${environment.apiUrl}/${this.followersUrl}/${username}`;
    return this.http.get<UserViewModel[]>(url);
  }

  followUser(username: string): Observable<string> {
    const url = `${environment.apiUrl}/${this.followUrl}/${username}`;
    return this.http.post(url, {}, { responseType: 'text' });
  }

  unfollowUser(username: string): Observable<string> {
    const url = `${environment.apiUrl}/${this.unfollowUrl}/${username}`;
    return this.http.post(url, {}, { responseType: 'text' });
  }
}