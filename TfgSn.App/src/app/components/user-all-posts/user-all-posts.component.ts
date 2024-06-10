import { Component, OnInit, ViewChild, ElementRef, QueryList, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { CommentsComponent } from '../comments/comments.component';
import { PostService } from 'src/app/services/post/post.service';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';
import { UserService } from 'src/app/services/user/user.service';
import { ChangeDetectorRef } from '@angular/core';
import { PostDto } from 'src/app/models/Post/PostDto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-all-posts',
  templateUrl: './user-all-posts.component.html',
  styleUrls: ['./user-all-posts.component.scss']
})
export class UserAllPostsComponent implements OnInit {
  userPosts: UserAndPostDto[] | undefined;
  profileImages: { [username: string]: string } = {};
  postImages: { [postId: number]: string } = {};
  allImagesLoaded = false;
  currentPage: number = 1;
  itemsPerPage: number = 10;
  pagedPosts: PostDto[] = [];
  showWelcomeSection: boolean = false;

  @ViewChildren('firstPost') firstPost: QueryList<ElementRef> | undefined;

  constructor(
    private userpostService: UserpostService,
    private dialog: MatDialog,
    private postService: PostService,
    private sharedService: SharedService,
    private userService: UserService,
    private changeDetectorRef: ChangeDetectorRef,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getUserPosts();
  }

  getUserPosts(): void {
    this.userpostService.getAllPosts().subscribe((posts: UserAndPostDto[]) => {
      this.userPosts = posts;
      this.loadProfileImages(posts);
      this.showWelcomeSection = true;
      this.updatePagedPosts();
      console.log(posts);
    });
  }

  loadProfileImages(posts: UserAndPostDto[]): void {
    let loadedCount = 0;
    const totalImages = posts.length * 2; // Cada usuario y cada post tienen una imagen

    posts.forEach(post => {
      if (post.image) {
        this.userService.downloadUploadedImage(post.userName)
          .subscribe({
            next: (response) => {
              const blob = new Blob([response.body as BlobPart], { type: 'image/jpeg' });
              this.profileImages[post.userName] = URL.createObjectURL(blob);
              loadedCount++;
              if (loadedCount === totalImages) {
                this.allImagesLoaded = true;
              }
            },
            error: (error) => {
              console.error('Error descargando la imagen de perfil', error);
              this.profileImages[post.userName] = '../../assets/flags/mifoto.jpg';
              loadedCount++;
              if (loadedCount === totalImages) {
                this.allImagesLoaded = true;
              }
            },
          });
      } else {
        loadedCount++;
        if (loadedCount === totalImages) {
          this.allImagesLoaded = true;
        }
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
                  loadedCount++;
                  if (loadedCount === totalImages) {
                    this.allImagesLoaded = true;
                  }
                },
                error: (error) => {
                  console.error('Error descargando la imagen del post', error);
                  loadedCount++;
                  if (loadedCount === totalImages) {
                    this.allImagesLoaded = true;
                  }
                },
              });
          } else {
            loadedCount++;
            if (loadedCount === totalImages) {
              this.allImagesLoaded = true;
            }
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

  updatePagedPosts(): void {
    if (this.userPosts) {
      const allPosts = this.flattenAndSortPosts(this.userPosts);
      const startIndex = (this.currentPage - 1) * this.itemsPerPage;
      const endIndex = startIndex + this.itemsPerPage;
      this.pagedPosts = allPosts.slice(startIndex, endIndex);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagedPosts();
      this.scrollToTop();
    }
  }

  nextPage(): void {
    if (this.userPosts && this.currentPage < Math.ceil(this.userPosts.length / this.itemsPerPage)) {
      this.currentPage++;
      this.updatePagedPosts();
      this.scrollToTop();
    }
  }

  scrollToTop(): void {
    if (this.firstPost && this.firstPost.first) {
      this.firstPost.first.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
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

  navigateToOtherProfile(userName: string): void {
    this.router.navigate(['/main/other-profile', userName]);
  }
}