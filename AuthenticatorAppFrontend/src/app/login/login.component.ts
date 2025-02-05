import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ServicesService } from '../ApiServices/services.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonSuccessMessages } from '../common/CommonSuccessMessages';
import { CommonErrorMessages } from '../common/CommonErrorMessages';
import { CommonModule } from '@angular/common';

class LoginModel {
  Email!: string;
  Password!: string;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule,ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  showPassword = false; // Flag for password visibility toggle
  errorMessage!: string;
  isActive: boolean = false; // Optional flag for login container active state

  constructor(private formBuilder: FormBuilder, 
    private service: ServicesService, 
    private router: Router,
    private toaster: ToastrService) { }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });

    // Optional: Animate login container entrance on initialization
    setTimeout(() => this.isActive = true, 500); // Add a slight delay
  }

  onSubmit() {
    if (this.loginForm.valid) {
      let submitObj={
        email: this.loginForm.value.email,
        password:this.loginForm.value.password
      }

      this.service.login(submitObj).subscribe((data)=>{
        if(data!=null)
        {
          if(data.status==200){
            this.router.navigate(['/home']);
            this.toaster.success(CommonSuccessMessages.LoginSuccess);
            this.errorMessage = "";
            this.service.isLoggedIn = true;
            localStorage.setItem('UserEmail',this.loginForm.value.email);
          }
          else{
            this.router.navigate(['/']);
            this.toaster.error(CommonErrorMessages.LoginFailed);
            this.service.isLoggedIn = false;
            this.errorMessage = CommonErrorMessages.LoginFailed;
            this.loginForm.reset();
          }
        }
      })
    }
    else{
      console.error("Form Invalid");
    }
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
}
