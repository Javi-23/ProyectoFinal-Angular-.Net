import { CommentDTO } from "../Comment/CommentDTO";
import { Comments } from "../Interfaces/Comment";
import { LikesDTO } from "../Likes/LikesDto"


export interface PostDto {
    id: number;
    userName: string;
    text: string;
    image: Uint8Array;
    creationDate: Date;
    description: string;
    comments: Comments[];
    likes: LikesDTO[];
    userHasLiked?: boolean;
  }