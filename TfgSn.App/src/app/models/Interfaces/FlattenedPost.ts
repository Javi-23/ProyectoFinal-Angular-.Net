import { LikesDTO } from "../Likes/LikesDto";
import { Comments } from "./Comment";

export interface FlattenedPost {
    id: number;
    userName: string;
    text: string;
    creationDate: Date;
    description: string;
    comments: Comments[];
    likes: LikesDTO[];
  }