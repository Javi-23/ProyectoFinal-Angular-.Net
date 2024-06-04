import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AutheticationService } from '../../services/authetication.service';
import { Login } from '../../models/login';
import { JwtAuth } from '../../models/jwtAuth';
import { Register } from 'src/app/models/register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  hide = false;
  loginDto: Register = new Register();
  jwtDto: JwtAuth = new JwtAuth();

  constructor(private fb: FormBuilder, private authService: AutheticationService) {
    this.registerForm = this.fb.group({
      userName: ['', [Validators.required, Validators.pattern(/^[a-zA-Z]+$/)]],
      email: ['', [Validators.required, Validators.email]],
      description: [''],
      password: ['', [Validators.required]]
    });
  }

  ngOnInit() {}

  onRegister() {
    if (this.registerForm.invalid) {
      return;
    }

    console.log('register');
    const loginData = this.registerForm.value;
    console.log(loginData);
    this.authService.register(loginData);
  }
}