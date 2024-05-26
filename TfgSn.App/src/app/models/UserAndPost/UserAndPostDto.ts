import { PostDto } from "../Post/PostDto";


export interface UserAndPostDto {
  userId: string;
  userName: string;
  description: string;
  image: Uint8Array;
  posts: PostDto[];
}