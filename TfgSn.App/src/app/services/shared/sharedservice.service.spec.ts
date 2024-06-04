import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { PostDto } from 'src/app/models/Post/PostDto';
import { UserViewModel } from 'src/app/models/User/UserViewModel';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private filteredUsersSubject: BehaviorSubject<UserViewModel[]> = new BehaviorSubject<UserViewModel[]>([]);
  filteredUsers$: Observable<UserViewModel[]> = this.filteredUsersSubject.asObservable();
  private profileImageUpdatedSource = new Subject<void>();
  profileImageUpdated$ = this.profileImageUpdatedSource.asObservable();
  private userId: string = "";
  private postLikesSubject: BehaviorSubject<PostDto[]> = new BehaviorSubject<PostDto[]>([]);
  postLikes$ = this.postLikesSubject.asObservable();


  constructor() { }

  setFilteredUsers(users: UserViewModel[]) {
    this.filteredUsersSubject.next(users);
  }

  emitProfileImageUpdated() {
    this.profileImageUpdatedSource.next();
  }

  setUserId(userId: string): void {
    this.userId = userId;
  }

  getUserId(): string {
    return this.userId;
  }

  updatePostLikes(posts: PostDto[]) {
    this.postLikesSubject.next(posts);
  }
}