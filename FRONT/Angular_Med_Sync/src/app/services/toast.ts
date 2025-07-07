import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private toastContainer = document.getElementById('toast-container');

  showToast(message: string, type: 'success' | 'error' | 'warning' | 'info' = 'success') {
    if (!this.toastContainer) return;

    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    let icon = '';

    switch(type) {
      case 'success': icon = '<i class="fas fa-check-circle"></i>'; break;
      case 'error': icon = '<i class="fas fa-times-circle"></i>'; break;
      case 'warning': icon = '<i class="fas fa-exclamation-triangle"></i>'; break;
      case 'info': icon = '<i class="fas fa-info-circle"></i>'; break;
    }

    toast.innerHTML = `${icon}<span>${message}</span>`;

    this.toastContainer.appendChild(toast);

    setTimeout(() => toast.classList.add('show'), 100);
    setTimeout(() => {
      toast.classList.remove('show');
      toast.addEventListener('transitionend', () => toast.remove());
    }, 5000);
  }

  success(message: string) { this.showToast(message, 'success'); }
  error(message: string) { this.showToast(message, 'error'); }
  warning(message: string) { this.showToast(message, 'warning'); }
  info(message: string) { this.showToast(message, 'info'); }
}
