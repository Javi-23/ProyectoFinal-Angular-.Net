import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AutheticationService } from '../../services/authetication.service';
import { Login } from '../../models/login';
import { JwtAuth } from '../../models/jwtAuth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  hide = false;
  loginDto: Login = new Login();
  jwtDto: JwtAuth = new JwtAuth();
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private authService: AutheticationService, private router: Router) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z]+$/)]],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit() {}

  onLogin() {
    if (this.loginForm.invalid) {
      return;
    }
    const loginData = this.loginForm.value;
    this.authService.login(loginData).subscribe(
      (jwtDto) => {
        localStorage.setItem('jwtToken', jwtDto.token);
        this.router.navigate(['/main/posts']);
      },
      (error) => {
        this.errorMessage = error;
      }
    );
  }
}