import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CommentDTO } from 'src/app/models/Comment/CommentDTO';
import { PostDto } from 'src/app/models/Post/PostDto';
import { environment } from 'src/environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class PostService {
  createCommentUrl = 'Post/create-comment';
  createPostUrl = 'Post/create-post';
  likePostUrl = 'Post/like-post';
  unLikePostUrl = 'Post/unlike-post';
  downloadUploadedImageUrl = 'Post/download-uploaded-image';
  deletePostUrl = 'Post/delete-post';
  deleteCommentUrl = 'Post/delete-comment';

  constructor(private http: HttpClient) { }

  createComment(postId: number, text: string): Observable<CommentDTO> {
    const url = `${environment.apiUrl}/${this.createCommentUrl}/${postId}`;
    return this.http.post<CommentDTO>(url, `"${text}"`, { headers: { 'Content-Type': 'application/json' } });
  }

  createPost(formData: FormData): Observable<PostDto> {
    return this.http.post<PostDto>(`${environment.apiUrl}/${this.createPostUrl}`, formData);
  }

  likePost(postId: number): Observable<boolean> {
    const url = `${environment.apiUrl}/${this.likePostUrl}/${postId}`;
    return this.http.post<boolean>(url, null, { headers: { 'Content-Type': 'application/json' } });
  }

  unlikePost(postId: number): Observable<boolean> {
    const url = `${environment.apiUrl}/${this.unLikePostUrl}/${postId}`;
    return this.http.post<boolean>(url, null, { headers: { 'Content-Type': 'application/json' } });
  }

  downloadUploadedImage(postId: number): Observable<Blob> {
    const url = `${environment.apiUrl}/${this.downloadUploadedImageUrl}?postId=${postId}`;
    return this.http.get(url, { responseType: 'blob' });
  }

  deletePost(postId: number): Observable<boolean> {
    const url = `${environment.apiUrl}/${this.deletePostUrl}/${postId}`;
    return this.http.delete<boolean>(url);
  }

  deleteComment(commentId: number): Observable<boolean> {
    const url = `${environment.apiUrl}/${this.deleteCommentUrl}/${commentId}`;
    return this.http.delete<boolean>(url);
  }
}
