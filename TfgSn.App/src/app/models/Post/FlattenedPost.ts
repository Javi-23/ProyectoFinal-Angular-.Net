import { CommentPost } from "../Interfaces/CommentPost";
import { LikesDTO } from "../Likes/LikesDto";

export interface FlattenedPost {
    id: number;
    userName: string;
    text: string;
    image: Uint8Array;
    creationDate: Date;
    description: string;
    comments: CommentPost[];
    likes: LikesDTO[];
  }