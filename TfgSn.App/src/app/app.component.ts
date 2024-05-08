import { Component } from '@angular/core';
import { Login } from './models/login';
import { Register } from './models/register';
import { JwtAuth } from './models/jwtAuth';
import { AutheticationService } from './services/authetication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'TfgSn.App';
  loginDto = new Login();
  registerDto = new Register();
  jwtDto = new JwtAuth();

  constructor(private authService: AutheticationService) {}

  register(registerDto: Register) {
    this.authService.register(registerDto).subscribe();
  }

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
}
