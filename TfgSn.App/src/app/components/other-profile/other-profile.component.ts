import { Component } from '@angular/core';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user/user.service';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { FollowsService } from 'src/app/services/follows/follows.service';
import { FollowDialogComponent } from '../follow-dialog/follow-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CommentsComponent } from '../comments/comments.component';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';
import { PostService } from 'src/app/services/post/post.service';
import { ChangeDetectorRef } from '@angular/core';
import { PostDto } from 'src/app/models/Post/PostDto';

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
  profileImageUrl: string | null = null;
  postImages: { [postId: number]: string } = {};

  constructor(private userpostService: UserpostService, private userService: UserService, 
              private followService: FollowsService, private dialog: MatDialog, private route: ActivatedRoute, 
              private router: Router, private sharedService: SharedService, private postService: PostService,
              private changeDetectorRef: ChangeDetectorRef) { }

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
        if (this.username == this.usernameAuthenticated) {
          this.router.navigateByUrl("/main/profile");
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
        if (this.user.image) {
          this.downloadProfileImage();
        } else {
          this.setDefaultProfileImage();
        }     
      });
  }

  getUserPosts(): void {
    this.userpostService.getUserPost(this.username)
      .subscribe(posts => {
        this.userPosts = posts;
        this.downloadPostImages(this.userPosts.posts);
      });
  }

  getFollowedUsers(): void {
    this.followService.getFollowed(this.username)
      .subscribe(followed => {
        this.userFollowed = followed;
      });
  }

  getFollowerUsers(): void {
    this.followService.getFollowers(this.username)
      .subscribe(follower => {
        this.userFollower = follower;
        this.checkFollowing();
      });
  }

  checkFollowing() {
    if (!this.userFollower) {
      return;
    }
    const currentUserFollows = this.userFollower.some(follower => follower.userName === this.userAuthenticated?.userName);
    this.isFollowing = currentUserFollows;
  }

  flattenAndSortPosts(userPosts: UserAndPostDto): any[] {
    const userId = this.sharedService.getUserId();
    return userPosts.posts.map(post => ({
      ...post,
      userName: userPosts.userName,
      description: userPosts.description,
      likes: post.likes,
      userHasLiked: post.likes.some(like => like.userId === userId),
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
    this.dialog.open(FollowDialogComponent, {
      width: '400px',
      data: { type: 'Seguidos', username: this.username, users: this.flattenUsers(this.userFollowed!) }
    });
  }

  showFollower(): void {
    this.dialog.open(FollowDialogComponent, {
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
          this.isFollowing = false;
          this.getFollowerUsers();
        },
        error: (error) => {
          console.error('Error al seguir al usuario:', error);
        }
      });
  }

  openCommentsDialog(post: PostDto): void {
    this.dialog.open(CommentsComponent, {
      width: '500px',
      data: { post }
    });
  }

  toggleLike(post: PostDto): void {
    this.postService.likePost(post.id).subscribe(response => {
      const userId = this.sharedService.getUserId();
      post.userHasLiked = response;

      if (response) {
        if (!post.likes.some(like => like.userId === userId)) {
          post.likes.push({
            userId: userId,
            id: 0,
            postId: post.id
          });
        }
      } else {
        const index = post.likes.findIndex(like => like.userId === userId);
        if (index !== -1) {
          post.likes.splice(index, 1);
        }
      }
    });
  }

  downloadProfileImage(): void {
    this.userService.downloadUploadedImage(this.username)
      .subscribe({
        next: (response) => {
          const blob = new Blob([response.body as BlobPart], { type: 'image/jpeg' });
          this.profileImageUrl = URL.createObjectURL(blob);
          this.changeDetectorRef.detectChanges();
        },
        error: (error) => {
          console.error('Error descargando la imagen de perfil', error);
        },
      });
  }

  setDefaultProfileImage(): void {
    this.profileImageUrl = '../../assets/flags/mifoto.jpg';
  }

  downloadPostImages(posts: PostDto[]): void {
    posts.forEach((post: PostDto) => {
      if (post.image) {
        this.postService.downloadUploadedImage(post.id)
          .subscribe({
            next: (response) => {
              const blob = new Blob([response as BlobPart], { type: 'image/jpeg' });
              this.postImages[post.id] = URL.createObjectURL(blob);
              this.changeDetectorRef.detectChanges();
            },
            error: (error) => {
              console.error('Error descargando la imagen del post', error);
            },
          });
      }
    });
  }
}