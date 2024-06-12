import { Component, ElementRef, OnInit, QueryList, ViewChildren } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { CommentsComponent } from '../../comments/comments.component';
import { PostService } from 'src/app/services/post/post.service';
import { UserService } from 'src/app/services/user/user.service';
import { ChangeDetectorRef } from '@angular/core';
import { PostDto } from 'src/app/models/Post/PostDto';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared/sharedservice.service.spec';

@Component({
  selector: 'app-followedposts',
  templateUrl: './followedposts.component.html',
  styleUrls: ['./followedposts.component.scss']
})
export class FollowedpostsComponent implements OnInit {
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
    this.userpostService.getFollowedPost().subscribe((posts: UserAndPostDto[]) => {
      this.userPosts = posts;
      this.loadProfileImages(posts);
      this.updatePagedPosts();
      this.showWelcomeSection = true;
    });
  }

  loadProfileImages(posts: UserAndPostDto[]): void {
    let totalImagesToLoad = 0;
    let loadedImages = 0;
    posts.forEach(post => {
      if (post.image) {
        totalImagesToLoad++;
        this.userService.downloadUploadedImage(post.userName)
          .subscribe({
            next: (response) => {
              const blob = new Blob([response.body as BlobPart], { type: 'image/jpeg' });
              this.profileImages[post.userName] = URL.createObjectURL(blob);
              loadedImages++;
              this.checkAllImagesLoaded(totalImagesToLoad, loadedImages);
            },
            error: (error) => {
              console.error('Error descargando la imagen de perfil', error);
              this.profileImages[post.userName] = '../../../../assets/flags/mifoto.jpg';
              loadedImages++;
              this.checkAllImagesLoaded(totalImagesToLoad, loadedImages);
            },
          });
      }
    });

    if (totalImagesToLoad === 0) {
      this.allImagesLoaded = true;
    }
  }

  downloadPostImages(posts: PostDto[]): void {
    let totalImagesToLoad = 0;
    let loadedImages = 0;
    posts.forEach((post: PostDto) => {
      if (post.image) {
        totalImagesToLoad++;
        this.postService.downloadUploadedImage(post.id)
          .subscribe({
            next: (response) => {
              const blob = new Blob([response as BlobPart], { type: 'image/jpeg' });
              this.postImages[post.id] = URL.createObjectURL(blob);
              loadedImages++;
              this.checkAllImagesLoaded(totalImagesToLoad, loadedImages);
            },
            error: (error) => {
              console.error('Error descargando la imagen del post', error);
              loadedImages++;
              this.checkAllImagesLoaded(totalImagesToLoad, loadedImages);
            },
          });
      }
    });

    if (totalImagesToLoad === 0) {
      this.allImagesLoaded = true;
    }
  }

  checkAllImagesLoaded(totalImages: number, loadedImages: number): void {
    if (totalImages === loadedImages) {
      this.allImagesLoaded = true;
      this.changeDetectorRef.detectChanges();
    }
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
    )
    .sort((a, b) => {
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
      this.downloadPostImages(this.pagedPosts); // Call this to load images for the paged posts
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
    return new Date(date).toLocaleString();
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

  navigateToOtherProfile(username: string): void {
    this.router.navigateByUrl(`/main/other-profile/${username}`);
  }
}