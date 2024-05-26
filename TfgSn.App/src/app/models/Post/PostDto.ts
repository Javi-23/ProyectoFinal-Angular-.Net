import { CommentDTO } from "../Comment/CommentDTO";
import { LikesDTO } from "../Likes/LikesDto"


export interface PostDto {
    id: number;
    userName: string;
    text: string;
    creationDate: Date;
    comments: CommentDTO[];
    likes: LikesDTO[];
  }