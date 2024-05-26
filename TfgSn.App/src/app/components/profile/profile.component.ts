import { ChangeDetectorRef, Component } from '@angular/core';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user/user.service';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { FollowsService } from 'src/app/services/follows/follows.service';
import { FollowDialogComponent } from '../follow-dialog/follow-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent {
  userPosts: UserAndPostDto | undefined;
  userFollowed: UserViewModel[] | undefined;
  userFollower: UserViewModel[] | undefined;
  user: UserViewModel | undefined;
  username: string = "";
  isUploadImageModalVisible = false;
  profileImageUrl: string | null = null;

  constructor(private userpostService: UserpostService,
     private userService: UserService, private followService: FollowsService, private dialog: MatDialog, 
     private changeDetectorRef: ChangeDetectorRef, private snackBar: MatSnackBar, private sharedService: SharedService) { }

  ngOnInit(): void {
    this.getUser();
  }

  getUser(): void {
    this.userService.getUser()
      .subscribe(user => {
        this.user = user;
        this.username = this.user?.userName ?? "";
        this.getUserPosts(); 
        this.getFollowedUsers();
        this.getFollowerUsers();
        if (this.user.image) {
          this.downloadProfileImage()
        } else {
          this.setDefaultProfileImage()
        }
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
      console.log(follower)
    })
  }


  flattenAndSortPosts(userPosts: UserAndPostDto): any[] {
    return userPosts.posts.map(post => ({
      ...post,
      userName: userPosts.userName,
      description: userPosts.description,
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

  

  UploadImageModal(): void {
    this.isUploadImageModalVisible = !this.isUploadImageModalVisible;
  }

  onImageUploaded(image: Uint8Array): void {
    if (this.user) {
      this.user.image = image;
      this.downloadProfileImage();
      this.sharedService.emitProfileImageUpdated();
      this.snackBar.open('Imagen de perfil actualizada', '', {
        duration: 2000,
      });
      this.changeDetectorRef.detectChanges();
    }
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
}



  // toggleComments(post: any): void {
  //   post.showComments = !post.showComments;
  // }
  
  // openCommentsDialog(post: any): void {
  //   const dialogRef = this.dialog.open(CommentsComponent, {
  //     width: '500px',
  //     data: { post }
  //   });
  // }
