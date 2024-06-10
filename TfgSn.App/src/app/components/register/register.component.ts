import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AutheticationService } from '../../services/authetication.service';
import { Register } from '../../models/register';  // Asegúrate de que este modelo existe

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  hide = false;
  registerDto: Register = new Register();
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder, 
    private authService: AutheticationService, 
    private router: Router
  ) {
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
    const registerData = this.registerForm.value;
    this.authService.register(registerData).subscribe({
      next: () => {
        this.router.navigate(['']); 
      },
      error: (error) => {
        this.errorMessage = error;
      }
    });
  }
  
  togglePasswordVisibility() {
    this.hide = !this.hide;
  }
}