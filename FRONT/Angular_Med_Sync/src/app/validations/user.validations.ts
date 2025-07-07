import { AbstractControl, ValidationErrors, ValidatorFn, FormGroup } from '@angular/forms';

export class UserValidators {
  static passwordStrength(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) return null;

      const hasUpperCase = /[A-Z]/.test(value);
      const hasLowerCase = /[a-z]/.test(value);
      const hasNumeric = /[0-9]/.test(value);

      const valid = hasUpperCase && hasLowerCase && hasNumeric;
      return valid ? null : { passwordStrength: true };
    };
  }

  static passwordMatchValidator: ValidatorFn = (form: AbstractControl): ValidationErrors | null => {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordsMismatch: true };
    }
    return null;
  };

  static getFormValidationMessage(form: FormGroup): string {
    const messages: string[] = [];

    if (form.errors?.['passwordsMismatch']) {
      messages.push('As senhas não coincidem.<br>');
    }

    for (const field in form.controls) {
      if (!form.controls.hasOwnProperty(field)) continue;

      const control = form.get(field);
      if (control && control.invalid && control.errors) {
        for (const errorKey in control.errors) {
          if (!control.errors.hasOwnProperty(errorKey)) continue;

          let message = '';

          if (errorKey === 'required') {
            message = `O campo ${field} é obrigatório<br>`;
          } else if (errorKey === 'email') {
            message = `O campo ${field} deve ser um E-mail válido<br>`;
          } else if (errorKey === 'minlength') {
            const requiredLength = control.errors['minlength'].requiredLength;
            message = `O campo ${field} deve ter pelo menos ${requiredLength} caracteres<br>`;
          } else if (errorKey === 'passwordStrength') {
            message = `O campo ${field} deve conter letras maiúsculas, minúsculas e números<br>`;
          } else {
            message = `O campo ${field} está inválido.<br>`;
          }

          messages.push(message);
        }
      }
    }

    return messages.join(' ');
  }
}
