import { ChangeDetectorRef, Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router'
import { Location } from '@angular/common';
import { userItems } from './header-dummy-data';
import { UserViewModel } from '../models/User/UserViewModel';
import { UserService } from '../services/user/user.service';
import { SharedService } from '../services/shared/sharedservice.service.spec';
import { Dialog } from '@angular/cdk/dialog';
import { SettingsModalComponent } from '../components/settings-modal/settings-modal.component';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Input() collapsed = false;
  @Input() screenWidth = 0;

  searchTerm = '';
  CanShowSearchCanOverlay = false;
  path = '';

  userItems = userItems;
  user: UserViewModel | undefined;
  username: string = "";
  profileImageUrl: string | null = null;
  
  constructor(private router: Router, private location: Location, private userService : UserService, private sharedService: SharedService, 
    private dialog: Dialog, private changeDetectorRef: ChangeDetectorRef) {
    this.router.events.subscribe((val) => {
      this.path = this.location.path();
    });
  }
  
  @HostListener('window:resize', ['$event'])
  OnResize(event: any) {
    this.checkCanShowSerachAsOverlay(window.innerWidth);
  }

  ngOnInit(): void {
    this.checkCanShowSerachAsOverlay(window.innerWidth);
    this.getUser();
    this.sharedService.profileImageUpdated$.subscribe(() => {
      this.downloadProfileImage();
    });
  }

  getUser(): void {
    this.userService.getUser()
      .subscribe(user => {
        this.user = user;
        this.username = this.user?.userName ?? "";
        if (this.user.image) {
          this.downloadProfileImage();
        } else {
          this.setDefaultProfileImage();
        }
      });
  }


  getHeadClass(): string {
    let styleClass='';
    if(this.collapsed && this.screenWidth > 768){
      styleClass = 'head-trimmed'
    } else {
      styleClass = 'head-md-screen'
    }
    return styleClass;
  }

  checkCanShowSerachAsOverlay(innerWidth: number): void {
    if(innerWidth < 845) {
      this.CanShowSearchCanOverlay = true;
    } else {
      this.CanShowSearchCanOverlay = false;
    }
  }

  redirectUsersComponent(): void {
    this.router.navigate(['/users']);
  }
  
  search(termine: string) {
    if (termine.trim() !== '') {
      this.userService.getUsersByPrefix(termine.trim())
        .subscribe(data => {
          this.sharedService.setFilteredUsers(data);
        });
    } else {
      this.sharedService.setFilteredUsers([]);
    }
  }

  openSettings() {
    this.dialog.open(SettingsModalComponent, {
      width: '400px'
    });
  }

  handleMenuClick(item: any) {
    if (item.label === 'Ajustes') {
      this.openSettings();
    } else if (item.label === 'Cerrar Sesión') {
      this.logout();
    } else if (item.route) {
      this.router.navigate([item.route]);
    }
  }
  
  logout() {
    const confirmLogout = confirm('¿Estás seguro de que deseas cerrar sesión?');
    if (confirmLogout) {
      localStorage.removeItem('jwtToken');
      this.router.navigate(['']);
    }
  }

  downloadProfileImage(): void {
    this.userService.downloadUploadedImage(this.username)
      .subscribe({
        next: (response) => {
          const blob = new Blob([response.body as BlobPart], { type: 'image/jpeg' });
          this.profileImageUrl = URL.createObjectURL(blob);
          this.changeDetectorRef.detectChanges();
        },
        error: (error) => {
          console.error('Error descargando la imagen de perfil', error);
        },
      });
  }

  setDefaultProfileImage(): void {
    this.profileImageUrl = '../../assets/flags/mifoto.jpg';
    this.changeDetectorRef.detectChanges();
  }
}



