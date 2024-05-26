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

  constructor(private http: HttpClient) { }

  createComment(postId: number, text: string): Observable<CommentDTO> {
    const url = `${environment.apiUrl}/${this.createCommentUrl}/${postId}`;
    return this.http.post<CommentDTO>(url, `"${text}"`, { headers: { 'Content-Type': 'application/json' } });
  }

  createPost(text: string): Observable<PostDto> {
    const url = `${environment.apiUrl}/${this.createPostUrl}`;
    const body = { text };
    return this.http.post<PostDto>(url, body, { headers: { 'Content-Type': 'application/json' } });
  }

  likePost(postId: number): Observable<boolean> {
    const url = `${environment.apiUrl}/${this.likePostUrl}/${postId}`;
    return this.http.post<boolean>(url, null, { headers: { 'Content-Type': 'application/json' } });
  }


}
