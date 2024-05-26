import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserAndPostDto } from 'src/app/models/UserAndPost/UserAndPostDto';
import { UserpostService } from 'src/app/services/user-post/userpost.service';
import { CommentsComponent } from '../comments/comments.component';
import { FlattenedPost } from 'src/app/models/Interfaces/FlattenedPost';


@Component({
  selector: 'app-user-all-posts',
  templateUrl: './user-all-posts.component.html',
  styleUrls: ['./user-all-posts.component.scss']
})
export class UserAllPostsComponent {
  userPosts: UserAndPostDto[] | undefined;
  postCommentsNotEmpty = false;

  constructor(private userpostService: UserpostService, private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getUserPosts();
  }

  getUserPosts(): void {
    this.userpostService.getAllPosts()
      .subscribe((posts: UserAndPostDto[]) => {
        this.userPosts = posts;
        console.log(posts);
      });
  }

  flattenAndSortPosts(userPosts: UserAndPostDto[]): FlattenedPost[] {
    return userPosts.flatMap(userPost =>
      userPost.posts.map(post => ({
        ...post,
        userName: userPost.userName,
        description: userPost.description,
        comments: post.comments,
        likes: post.likes,
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

  toggleComments(post: any): void {
    post.showComments = !post.showComments;
  }
  
  openCommentsDialog(post: FlattenedPost): void {
    const dialogRef = this.dialog.open(CommentsComponent, {
      width: '500px',
      data: { post }
    });
  }

  formatDate(date: Date): string {
    return date.toLocaleString();
  }
}