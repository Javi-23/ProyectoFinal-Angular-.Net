import { LikesDTO } from "../Likes/LikesDto";
import { CommentPost } from "./CommentPost";

export interface FlattenedPostPost {
    id: number;
    userName: string;
    text: string;
    creationDate: Date;
    description: string;
    comments: CommentPost[];
    likes: LikesDTO[];
  }