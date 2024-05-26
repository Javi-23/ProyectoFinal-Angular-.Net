import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { UserAllPostsComponent } from './components/user-all-posts/user-all-posts.component';
import { FollowedpostsComponent } from './components/followed-posts/followedposts/followedposts.component';
import { ProfileComponent } from './components/profile/profile.component';
import { OtherProfileComponent } from './components/other-profile/other-profile.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'posts', component: UserAllPostsComponent },
  { path: 'followed', component: FollowedpostsComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'other-profile/:username', component: OtherProfileComponent },
  { path: "users", component: UsersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }