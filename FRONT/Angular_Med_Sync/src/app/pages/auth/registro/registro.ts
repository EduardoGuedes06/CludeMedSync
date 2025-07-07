import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../../../services/auth';
import { UserValidators } from '../../../validations/modelsValidations/user.validations';
import { attachValidationHandlers } from '../../../validations/inputValidations/formValidators';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './registro.html',
  styleUrls: ['./registro.css']
})
export class Registro implements AfterViewInit {
  registroForm: FormGroup;

  constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
    this.registroForm = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        UserValidators.passwordStrength()
      ]],
      confirmPassword: ['', Validators.required]
    }, { validators: Registro.passwordMatchValidator });
  }

  ngAfterViewInit() {
    attachValidationHandlers();
  }

  static passwordMatchValidator: ValidatorFn = (form: AbstractControl): ValidationErrors | null => {
    const senha = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (senha && confirmPassword && senha !== confirmPassword) {
      return { passwordsMismatch: true };
    }
    return null;
  };

  registrar() {
    if (this.registroForm.invalid) {
      this.registroForm.markAllAsTouched();
      const errorMessage = UserValidators.getFormValidationMessage(this.registroForm);
      if (this.registroForm.errors?.['passwordsMismatch']) {
        (window as any).showToast('As senhas não coincidem.', 'warning');
      } else {
        (window as any).showToast(errorMessage || 'Preencha corretamente o formulário.', 'warning');
      }
      return;
    }

    const { nome, email, password, confirmPassword } = this.registroForm.value;
    this.auth.register(nome, email, password, confirmPassword).subscribe({
      next: () => {
        (window as any).showToast('Registro realizado com sucesso!', 'success');
        this.router.navigate(['/auth/login'], { queryParams: { email } });
      },
      error: (err) => {
        (window as any).showToast('Falha no registro: ' + err.message, 'error');
      }
    });
  }
}
