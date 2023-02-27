import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });

  constructor(
    private accountService: AccountService,
    private router: Router,
  ) {}

  onSubmit() {
    this.accountService
      .login(this.loginForm.value)
      .subscribe({
        next: () => this.router.navigateByUrl('/shop'),
      });
  }
}
