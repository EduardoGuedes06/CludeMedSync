import React from 'react';

interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  title: string;
  children: React.ReactNode;
  icon?: React.ReactNode;
}

const Card = ({ title, icon, children, className, ...props }: CardProps) => {
  return (
    <div
      className={`bg-white p-6 rounded-lg shadow-md transition-shadow hover:shadow-lg ${className}`}
      {...props}
    >
      <div className="flex items-center justify-between mb-4">
        <h3 className="text-lg font-semibold text-gray-700">{title}</h3>
        {icon && <div className="text-brand-primary">{icon}</div>}
      </div>
      <div>{children}</div>
    </div>
  );
};

export { Card };
