import { Component } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, finalize, map, switchMap, take } from 'rxjs';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent {
  errors: string[] | null = null;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
  ) {}

  // reference: https://regexlib.com/REDetails.aspx?regexp_id=1111
  complexPassword = "(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$";

  registerForm = this.fb.group({
    displayName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email], [this.validateEmailNotTaken()]],
    password: ['', [Validators.required, Validators.pattern(this.complexPassword)]],
  });

  onSubmit() {
    this.accountService
      .register(this.registerForm.value)
      .subscribe({
        next: () => this.router.navigateByUrl('/shop'),
        error: (error) => this.errors = error.errors,
      });
  }

  validateEmailNotTaken(): AsyncValidatorFn {
    return (control: AbstractControl) => (
      control.valueChanges.pipe(
        debounceTime(500), // debounce API calls to async validator by 0.5s
        take(1), // take the most recent result (i.e., on user typing of field `email`)
        switchMap(() => (
          this.accountService
          .checkEmailExists(control.value)
          .pipe(
            map((result) => result ? { emailExists: true } : null),
            finalize(() => control.markAsTouched()),
          )
        )),
      )
    );
  }
}
