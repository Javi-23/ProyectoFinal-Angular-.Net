import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AutheticationService } from '../../services/authetication.service';
import { Login } from '../../models/login';
import { JwtAuth } from '../../models/jwtAuth';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  hide = false;
  loginDto: Login = new Login();
  jwtDto: JwtAuth = new JwtAuth();

  constructor(private fb: FormBuilder, private authService: AutheticationService) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.pattern(/^[a-zA-Z]+$/)]],
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
    const loginData = this.registerForm.value;
    this.authService.register(loginData);
  }
}