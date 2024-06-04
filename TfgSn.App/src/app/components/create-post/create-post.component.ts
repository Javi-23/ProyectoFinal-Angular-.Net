import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { PostService } from 'src/app/services/post/post.service';
import { Observable } from 'rxjs';
import { PostDto } from 'src/app/models/Post/PostDto';

@Component({
  selector: 'app-create-post',
  templateUrl: './create-post.component.html',
  styleUrls: ['./create-post.component.scss']
})
export class CreatePostComponent {
  text: string = '';
  selectedFile: File | null = null;

  constructor(
    public dialogRef: MatDialogRef<CreatePostComponent>,
    private postService: PostService
  ) {}

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
    }
  }

  createPost(): void {
    if (this.text.trim() === '') {
      return;
    }

    const formData = new FormData();
    formData.append('Text', this.text);
    if (this.selectedFile) {
      formData.append('ImageFile', this.selectedFile);
    }

    this.postService.createPost(formData).subscribe({
      next: (newPost) => {
        console.log('Nueva publicación creada:', newPost);
        window.location.reload();
      },
      error: (error) => {
        console.error('Error al crear la publicación:', error);
      }
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}