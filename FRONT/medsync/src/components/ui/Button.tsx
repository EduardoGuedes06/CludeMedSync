import React, { forwardRef } from 'react';

// Usaremos a biblioteca `tailwind-merge` no futuro para evitar conflitos de classes
// Por enquanto, a lógica de classes é simples.

// Definimos as propriedades que nosso botão pode receber
export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'danger' | 'secondary';
  isLoading?: boolean;
}

const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant = 'primary', isLoading = false, children, ...props }, ref) => {
    
    // Lógica para definir as classes CSS com base nas props
    const baseClasses = "w-full py-2 px-4 rounded-md font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center";

    const variantClasses = {
      primary: 'bg-brand-primary text-white hover:bg-blue-700 focus:ring-brand-primary',
      danger: 'bg-brand-danger text-white hover:bg-red-700 focus:ring-brand-danger',
      secondary: 'bg-brand-secondary text-white hover:bg-gray-700 focus:ring-brand-secondary',
    };

    const combinedClasses = `${baseClasses} ${variantClasses[variant]} ${className || ''}`;

    return (
      <button ref={ref} className={combinedClasses} disabled={isLoading} {...props}>
        {isLoading ? (
          <>
            <svg className="animate-spin -ml-1 mr-3 h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
              <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            A processar...
          </>
        ) : (
          children
        )}
      </button>
    );
  }
);

Button.displayName = 'Button';

export { Button };