import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';

import { AuthService } from '../../../services/auth';
import { User } from '../../../models/user.model';
import { UserValidators } from '../../../validations/user.validations';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        UserValidators.passwordStrength()
      ]],
    });

    this.route.queryParams.subscribe(params => {
      const email = params['email'];
      if (email) {
        this.loginForm.patchValue({ email });
      }
    });
  }

  login() {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      const errorMessage = UserValidators.getFormValidationMessage(this.loginForm);
      (window as any).showToast(errorMessage, 'warning');
      return;
    }

    const { email, password } = this.loginForm.value as User;

    this.auth.login(email, password).subscribe({
      next: () => {
        (window as any).showToast('Login realizado com sucesso!', 'success');
        this.router.navigate(['/']);
      },
      error: (err) => {
        (window as any).showToast('Erro ao fazer login: ' + err.message, 'error');
      }
    });
  }
}
