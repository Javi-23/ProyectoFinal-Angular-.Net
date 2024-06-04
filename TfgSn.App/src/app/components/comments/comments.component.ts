import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PostService } from 'src/app/services/post/post.service'; 
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentPost } from 'src/app/models/Interfaces/CommentPost';
import { UserService } from 'src/app/services/user/user.service';
import { UserViewModel } from 'src/app/models/User/UserViewModel';
import { PostDto } from 'src/app/models/Post/PostDto';
import { FlattenedPost } from 'src/app/models/Post/FlattenedPost';

interface DialogData {
  comments: Comment;
  post: PostDto;
}

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit{

  post: FlattenedPost;
  commentText: string = '';
  showCommentForm: boolean = false;
  user: UserViewModel | undefined;
  username: string = "";

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private postService: PostService,
    private snackBar: MatSnackBar,
    private userService: UserService
  ) {
    this.post = data.post;
  }
  ngOnInit(): void {
    this.getUser();
    this.sortCommentsByCreationDate();
  }

  getUser(): void {
    this.userService.getUser()
      .subscribe(user => {
        this.user = user;
        this.username = this.user?.userName ?? "";
      });
  }

  toggleCommentForm(): void {
    this.showCommentForm = !this.showCommentForm;
  }

  createComment(): void {
    const username = this.username;

    const newComment: CommentPost = {
      text: this.commentText,
      userName: username,
      creationDate: new Date()
    };
    this.postService.createComment(this.post.id, this.commentText).subscribe({
      next: (response) => {
        this.post.comments.unshift(newComment);
        console.log('Comentario creado:', response);
        this.snackBar.open('Comentario creado con éxito', 'Cerrar', {
          duration: 3000,
          verticalPosition: 'bottom',
          horizontalPosition: 'center'
        });
      },
      error: (error) => {
        console.error('Error al crear el comentario:', error);
        this.snackBar.open('Error al crear el comentario', 'Cerrar', {
          duration: 3000,
          verticalPosition: 'bottom',
          horizontalPosition: 'center'
        });
      },
      complete: () => {
        console.log('La operación de crear comentario ha sido completada');
      }
    });
  }

  sortCommentsByCreationDate(): void {
    this.post.comments.sort((a, b) => {
      const dateA = new Date(a.creationDate).getTime();
      const dateB = new Date(b.creationDate).getTime();
      return dateB - dateA;
    });
  }
  
  formatDate(date: Date): string {
    return new Date(date).toLocaleString();
  }
}