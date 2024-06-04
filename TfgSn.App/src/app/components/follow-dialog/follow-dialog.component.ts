import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router, NavigationStart } from '@angular/router';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { FollowsService } from 'src/app/services/follows/follows.service';
import { UserService } from 'src/app/services/user/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

interface DialogData {
  type: string;
  username: string;
  users: UserViewModel[];
}

@Component({
  selector: 'app-follow-dialog',
  templateUrl: './follow-dialog.component.html',
  styleUrls: ['./follow-dialog.component.scss']
})
export class FollowDialogComponent {
  usernameAuthenticated: string = "";
  isAuthenticated = false;
  userFollowed: UserViewModel[] | undefined;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private followService: FollowsService,
    private userService: UserService,
    private router: Router,
    private dialogRef: MatDialogRef<FollowDialogComponent>,
    private snackBar: MatSnackBar
  ) {
    console.log('Datos recibidos:', JSON.stringify(data));

    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.dialogRef.close();
      }
    });

    this.getUserAuthenticated();
  }

  getUserAuthenticated(): void {
    this.userService.getUser()
      .subscribe(user => {
        this.usernameAuthenticated = user.userName;
        console.log('Usuario autenticado:', this.usernameAuthenticated);
        this.isAuthenticated = this.router.url === '/main/profile';

        this.getFollowedUsers();
      });
  }

  getFollowedUsers(): void {
    this.followService.getFollowed(this.usernameAuthenticated)
      .subscribe(followed => {
        this.userFollowed = followed;
        console.log(followed);
      });
  }

  navigateToOtherProfile(username: string): void {
    this.router.navigateByUrl(`main/other-profile/${username}`);
  }

  unfollowUser(userToUnfollow: UserViewModel): void {
    const confirmation = this.snackBar.open('¿Estás seguro de que quieres dejar de seguir a este usuario?', 'Dejar de seguir', {
      duration: 3000 
    });

    confirmation.afterDismissed().subscribe(result => {
      if (result.dismissedByAction) { 
        this.followService.unfollowUser(userToUnfollow.userName)
          .subscribe({
            next: (response: string) => {
              console.log('Se ha dejado de seguir al usuario', response);
              this.data.users = this.data.users.filter(user => user.userName !== userToUnfollow.userName);
            },
            error: (error) => {
              console.error('Error al dejar de seguir al usuario:', error);
            }
          });
      }
    });
  }
}