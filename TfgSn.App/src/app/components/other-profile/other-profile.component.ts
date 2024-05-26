import { Component } from '@angular/core';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user/user.service';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { FollowsService } from 'src/app/services/follows/follows.service';
import { FollowDialogComponent } from '../follow-dialog/follow-dialog.component';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { FlattenedPost } from 'src/app/models/Interfaces/FlattenedPost';

@Component({
  selector: 'app-other-profile',
  templateUrl: './other-profile.component.html',
  styleUrls: ['./other-profile.component.scss']
})
export class OtherProfileComponent {
  userPosts: UserAndPostDto | undefined;
  userFollowed: UserViewModel[] | undefined;
  userFollower: UserViewModel[] | undefined;
  user: UserViewModel | undefined;
  userAuthenticated: UserViewModel | undefined;
  username: string = "";
  isFollowing: boolean = false;
  usernameAuthenticated: string = "";

  constructor(private userpostService: UserpostService, private userService: UserService, private followService: FollowsService, private dialog: MatDialog, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.username = params['username'];
      this.getUserAuthenticated();
    });
  }

  getUserAuthenticated(): void {
    this.userService.getUser()
      .subscribe(user => {
        this.userAuthenticated = user;
        this.usernameAuthenticated = this.userAuthenticated?.userName ?? "";
        console.log(this.userAuthenticated);
        if (this.username == this.usernameAuthenticated) {
          this.router.navigateByUrl("/profile");
        }
        this.getUser();
      });
  }

  getUser(): void {
    this.userService.getUserByUsername(this.username)
      .subscribe(user => {
        this.user = user;
        this.getUserPosts(); 
        this.getFollowedUsers();
        this.getFollowerUsers();     
      });
  }

  getUserPosts(): void {
    this.userpostService.getUserPost(this.username)
      .subscribe(posts => {
        this.userPosts = posts;
        console.log(posts);
      });
  }

  getFollowedUsers(): void {
    this.followService.getFollowed(this.username)
    .subscribe(followed => {
      this.userFollowed = followed;
      console.log(followed)
    })
  }

  getFollowerUsers(): void {
    this.followService.getFollowers(this.username)
      .subscribe(follower => {
        this.userFollower = follower;
        console.log(follower);
        this.checkFollowing();
      });
  }

  checkFollowing() {
    if (!this.userFollower) {
      return;
    }
  
    const currentUserFollows = this.userFollower.some(follower => follower.userName === this.userAuthenticated?.userName);
    this.isFollowing = currentUserFollows;
    console.log("Usuario Autenticado: " + (this.userAuthenticated?.userName ?? "No definido")); 
    console.log("Usuario Seguidor: " + this.userFollower.map(follower => follower.userName).join(", ")); 
  }


  flattenAndSortPosts(userPosts: UserAndPostDto): FlattenedPost[] {
    return userPosts.posts.map(post => ({
      ...post,
      userName: userPosts.userName,
      description: userPosts.description,
      comments: post.comments
    })).sort((a, b) => new Date(b.creationDate).getTime() - new Date(a.creationDate).getTime());
  }

  flattenUsers(user: UserViewModel[]): UserViewModel[] {
    return user.map(user => ({
      ...user,
      userName: user.userName,
      description: user.description,
    }));
  }

  showFollowed(): void {
    const dialogRef = this.dialog.open(FollowDialogComponent, {
      width: '400px',
      data: { type: 'Seguidos', username: this.username, users: this.flattenUsers(this.userFollowed!) }
    });
  }

  showFollower(): void {
    const dialogRef = this.dialog.open(FollowDialogComponent, {
      width: '400px',
      data: { type: 'Seguidores', username: this.username, users: this.flattenUsers(this.userFollower!) }
    });
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleString();
  }

  followUser(): void {
    this.followService.followUser(this.username)
      .subscribe({
        next: (response: string) => {
          console.log('Usuario seguido correctamente:', response);
          this.isFollowing = true;
          this.getFollowerUsers();
        },
        error: (error) => {
          console.error('Error al seguir al usuario:', error);
        }
      });
  } 
  
  

  unfollowUser(): void {
    this.followService.unfollowUser(this.username)
      .subscribe({
        next: (response: string) => {
          console.log('Usuario seguido correctamente:', response);
          this.isFollowing = false;
          this.getFollowerUsers();
        },
        error: (error) => {
          console.error('Error al seguir al usuario:', error);
        }
      });
  } 
}
