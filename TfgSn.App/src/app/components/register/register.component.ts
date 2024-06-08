import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AutheticationService } from '../../services/authetication.service';
import { Register } from 'src/app/models/register';
import { JwtAuth } from 'src/app/models/jwtAuth';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  hide = true;
  loginDto: Register = new Register();
  jwtDto: JwtAuth = new JwtAuth();
  errorMessage: string | null = null;

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

    this.errorMessage = null;

    const loginData = this.registerForm.value;
    this.authService.register(loginData).subscribe({
      next: (response) => {
        console.log('Registro exitoso', response);
      },
      error: (error) => {
        this.errorMessage = error;
      }
    });
  }
}