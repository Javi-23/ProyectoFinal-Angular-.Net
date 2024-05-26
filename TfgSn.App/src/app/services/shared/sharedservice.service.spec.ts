import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { UserViewModel } from 'src/app/models/User/UserViewModel';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private filteredUsersSubject: BehaviorSubject<UserViewModel[]> = new BehaviorSubject<UserViewModel[]>([]);
  filteredUsers$: Observable<UserViewModel[]> = this.filteredUsersSubject.asObservable();
  private profileImageUpdatedSource = new Subject<void>();
  profileImageUpdated$ = this.profileImageUpdatedSource.asObservable();

  constructor() { }

  setFilteredUsers(users: UserViewModel[]) {
    this.filteredUsersSubject.next(users);
  }

  emitProfileImageUpdated() {
    this.profileImageUpdatedSource.next();
  }
}