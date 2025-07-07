window.showToast = function (message, type = 'success') {
  const container = document.getElementById('toast-container');
  if (!container) return;

  const toast = document.createElement('div');
  toast.className = `toast ${type}`;
  let icon = '';

  if (type === 'success') icon = '<i class="fas fa-check-circle"></i>';
  else if (type === 'error') icon = '<i class="fas fa-times-circle"></i>';
  else if (type === 'warning') icon = '<i class="fas fa-exclamation-triangle"></i>';
  else if (type === 'info') icon = '<i class="fas fa-info-circle"></i>';

  toast.innerHTML = `${icon}<span>${message}</span>`;

  container.appendChild(toast);
  setTimeout(() => toast.classList.add('show'), 100);
  setTimeout(() => {
    toast.classList.remove('show');
    toast.addEventListener('transitionend', () => toast.remove());
  }, 5000);
}
