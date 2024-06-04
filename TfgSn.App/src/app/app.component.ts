import { Component } from '@angular/core';
import { Router } from '@angular/router'; // Importa el Router

import { Login } from './models/login';
import { Register } from './models/register';
import { JwtAuth } from './models/jwtAuth';
import { AutheticationService } from './services/authetication.service';

interface SideNavToggle {
  screenWidth: number;
  collapsed: boolean;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'TfgSn.App';
  hideSidenav: boolean = false;
  
  IsSideNavCollapsed = false;
  screenWidth = 0;

  loginDto = new Login();
  registerDto = new Register();
  jwtDto = new JwtAuth();
  filteredUsers: any[] = [];

  constructor(
    private authService: AutheticationService,
    private router: Router // Inyecta el Router
  ) {}

  // register(registerDto: Register) {
  //   this.authService.register(registerDto).subscribe();
  // }

  login(loginDto: Login) {
    this.authService.login(loginDto).subscribe((jwtDto) => {
      localStorage.setItem('jwtToken', jwtDto.token);
    });
  }

  Weather() {
    this.authService.getWeather().subscribe((weatherdata: any) => {
      console.log(weatherdata);
    })
  }

  onToggleSideNav(data: SideNavToggle): void {
    this.screenWidth = data.screenWidth;
    this.IsSideNavCollapsed = data.collapsed;
  }

  onUserFilter(filteredData: any[]) {
    this.filteredUsers = filteredData;
  }
}