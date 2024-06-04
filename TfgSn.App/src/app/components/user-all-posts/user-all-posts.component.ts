import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { CommentsComponent } from '../comments/comments.component';
import { PostService } from 'src/app/services/post/post.service';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';
import { UserService } from 'src/app/services/user/user.service';
import { ChangeDetectorRef } from '@angular/core';
import { PostDto } from 'src/app/models/Post/PostDto';

@Component({
  selector: 'app-user-all-posts',
  templateUrl: './user-all-posts.component.html',
  styleUrls: ['./user-all-posts.component.scss']
})
export class UserAllPostsComponent implements OnInit {
  userPosts: UserAndPostDto[] | undefined;
  profileImages: { [username: string]: string } = {};
  postImages: { [postId: number]: string } = {};

  constructor(
    private userpostService: UserpostService,
    private dialog: MatDialog,
    private postService: PostService,
    private sharedService: SharedService,
    private userService: UserService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.getUserPosts();
  }

  getUserPosts(): void {
    this.userpostService.getAllPosts().subscribe((posts: UserAndPostDto[]) => {
      this.userPosts = posts;
      this.loadProfileImages(posts);
      console.log(posts);
    });
  }

  loadProfileImages(posts: UserAndPostDto[]): void {
    posts.forEach(post => {
      if (post.image) {
        this.userService.downloadUploadedImage(post.userName)
          .subscribe({
            next: (response) => {
              const blob = new Blob([response.body as BlobPart], { type: 'image/jpeg' });
              this.profileImages[post.userName] = URL.createObjectURL(blob);
              this.changeDetectorRef.detectChanges();
            },
            error: (error) => {
              console.error('Error descargando la imagen de perfil', error);
              this.profileImages[post.userName] = '../../assets/flags/mifoto.jpg';
            },
          });
      } else {
        this.profileImages[post.userName] = '../../assets/flags/mifoto.jpg';
      }

      if (post.posts) {
        post.posts.forEach((p: any) => {
          if (p.image) {
            this.postService.downloadUploadedImage(p.id)
              .subscribe({
                next: (response) => {
                  const blob = new Blob([response as BlobPart], { type: 'image/jpeg' });
                  this.postImages[p.id] = URL.createObjectURL(blob);
                  this.changeDetectorRef.detectChanges();
                },
                error: (error) => {
                  console.error('Error descargando la imagen del post', error);
                },
              });
          }
        });
      }
    });
  }

  flattenAndSortPosts(userPosts: UserAndPostDto[]): PostDto[] {
    const userId = this.sharedService.getUserId();
    return userPosts.flatMap(userPost =>
      userPost.posts.map(post => ({
        ...post,
        userName: userPost.userName,
        description: userPost.description,
        comments: post.comments,
        likes: post.likes,
        image: post.image,
        userHasLiked: post.likes.some(like => like.userId === userId),
        creationDate: (post.creationDate instanceof Date) ? post.creationDate : new Date(post.creationDate)
      }))
    ).sort((a, b) => {
      if (a.creationDate instanceof Date && b.creationDate instanceof Date) {
        return b.creationDate.getTime() - a.creationDate.getTime();
      } else {
        return 0;
      }
    });
  }

  toggleComments(post: any): void {
    post.showComments = !post.showComments;
  }

  openCommentsDialog(post: PostDto): void {
    this.dialog.open(CommentsComponent, {
      width: '500px',
      data: { post }
    });
  }

  formatDate(date: Date): string {
    return date.toLocaleString();
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
}