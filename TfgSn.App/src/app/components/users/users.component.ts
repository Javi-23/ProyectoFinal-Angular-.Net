import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent {
  @Input() filteredUsers: UserViewModel[] = [];

  constructor(private sharedService: SharedService, private router: Router) { }

  ngOnInit(): void {
    this.sharedService.filteredUsers$.subscribe(users => {
      this.filteredUsers = users;
    });
  }

  navigateToOtherProfile(username: string): void {
    this.router.navigateByUrl(`/other-profile/${username}`);
  }
}

