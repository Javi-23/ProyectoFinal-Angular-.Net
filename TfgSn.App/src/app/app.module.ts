import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'; // add

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms' // add
import { AuhtenticationInterceptor } from './services/interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatCardModule } from '@angular/material/card'
import { MatButtonModule } from '@angular/material/button'
import { MatInputModule } from '@angular/material/input'
import { MatIconModule } from '@angular/material/icon'
import { MatExpansionModule } from '@angular/material/expansion';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { UserAllPostsComponent } from './components/user-all-posts/user-all-posts.component';
import { CommentsComponent } from './components/comments/comments.component';
import { FollowedpostsComponent } from './components/followed-posts/followedposts/followedposts.component';
import { BodyComponent } from './components/body/body.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { HeaderComponent } from './header/header.component';
import { FollowDialogComponent } from './components/follow-dialog/follow-dialog.component';

import { OverlayModule } from '@angular/cdk/overlay';
import { CdkMenuModule } from '@angular/cdk/menu';
import { ProfileComponent } from './components/profile/profile.component';
import { OtherProfileComponent } from './components/other-profile/other-profile.component';
import { CreatePostComponent } from './components/create-post/create-post.component';
import { UsersComponent } from './components/users/users.component';
import { SettingsModalComponent } from './components/settings-modal/settings-modal.component';
import { ImageModalComponent } from './components/image-modal/image-modal.component';



@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    UserAllPostsComponent,
    CommentsComponent,
    BodyComponent,
    SidenavComponent,
    HeaderComponent,
    FollowedpostsComponent,
    ProfileComponent,
    FollowDialogComponent,
    OtherProfileComponent,
    CreatePostComponent,
    UsersComponent,
    SettingsModalComponent,
    ImageModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule, 
    FormsModule, 
    BrowserAnimationsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    ReactiveFormsModule,
    MatExpansionModule,
    MatListModule,
    MatSelectModule,
    MatDialogModule,
    MatSnackBarModule,
    OverlayModule,
    CdkMenuModule,
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: AuhtenticationInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
