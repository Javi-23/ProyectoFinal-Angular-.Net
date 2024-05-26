import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent {
  text: string = '';

  constructor(
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private postService: PostService
  ) {}

  createPost(): void {
    if (this.text.trim() !== '') { 
      this.postService.createPost(this.text).subscribe({
        next: (newPost) => {
          console.log('Nueva publicación creada:', newPost);
          window.location.reload();
        },
        error: (error) => {
          console.error('Error al crear la publicación:', error);
        }
      });
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}