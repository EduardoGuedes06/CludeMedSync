import { ValidationType } from './validation-types';

function handleNomeInput(input: HTMLInputElement) {
  const nameValue = input.value;
  const regex = /^[A-Za-zÀ-ÖØ-öø-ÿ. ]*$/;
  if (!regex.test(nameValue)) {
    input.value = nameValue.replace(/[^A-Za-zÀ-ÖØ-öø-ÿ. ]/g, '');
    input.setCustomValidity("O nome só pode conter letras e ponto.");
  } else {
    const correctedName = nameValue
      .toLowerCase()
      .replace(/(^|[\s.])([a-zà-öø-ÿ])/g, (match, sep, char) => sep + char.toUpperCase());
    input.value = correctedName;
    input.setCustomValidity('');
  }
  input.reportValidity();
}

function handleNumeroInput(input: HTMLInputElement) {
  let valor = input.value.replace(/\D/g, '');
  if (valor.length > 10) valor = valor.substring(0, 10);
  input.value = valor;
  input.setCustomValidity('');
  input.reportValidity();
}

function handleEmailInput(input: HTMLInputElement) {
  const value = input.value;
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(value)) {
    input.setCustomValidity("Informe um e-mail válido.");
  } else {
    input.setCustomValidity("");
  }
  input.reportValidity();
}

function handlePasswordInput(input: HTMLInputElement) {
  const value = input.value;

  const rules = {
    length: value.length >= 6,
    upper: /[A-Z]/.test(value),
    lower: /[a-z]/.test(value),
    number: /[0-9]/.test(value),
    special: /[!@#$%^&*(),.?":{}|<>]/.test(value)
  };

  let message = 'Sua senha deve conter:\n';
  message += rules.length ? '✔️ Mínimo 6 caracteres\n' : '❌ Mínimo 6 caracteres\n';
  message += rules.upper ? '✔️ Uma letra maiúscula\n' : '❌ Uma letra maiúscula\n';
  message += rules.lower ? '✔️ Uma letra minúscula\n' : '❌ Uma letra minúscula\n';
  message += rules.number ? '✔️ Um número\n' : '❌ Um número\n';
  message += rules.special ? '✔️ Um caractere especial (!@#$...)' : '❌ Um caractere especial (!@#$...)';

  if (Object.values(rules).every(Boolean)) {
    input.setCustomValidity('');
  } else {
    input.setCustomValidity(message);
  }
  input.reportValidity();
}


function attachValidationHandlers() {
  const inputs = document.querySelectorAll<HTMLInputElement>('[data-validate]');
  inputs.forEach(input => {
    const type = input.dataset['validate'] as ValidationType;
    if (!type) return;

    switch (type) {
      case 'nome':
        input.addEventListener('input', () => handleNomeInput(input));
        break;
      case 'numero':
        input.addEventListener('input', () => handleNumeroInput(input));
        break;
      case 'email':
        input.addEventListener('input', () => handleEmailInput(input));
        break;
      case 'password':
        input.addEventListener('input', () => handlePasswordInput(input));
        break;
      case 'confirmPassword':
        input.addEventListener('input', () => handlePasswordInput(input));
        break;
    }
  });
}

export { handleNomeInput, handleNumeroInput, attachValidationHandlers };
