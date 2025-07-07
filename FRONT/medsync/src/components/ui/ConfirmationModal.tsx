import React from 'react';
import { Modal } from './Modal';
import { Button } from './Button';
import { AlertTriangle } from 'lucide-react';

interface ConfirmationModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  description: string;
  onConfirm: () => void;
  isConfirming?: boolean;
}

const ConfirmationModal = ({
  open,
  onOpenChange,
  title,
  description,
  onConfirm,
  isConfirming = false,
}: ConfirmationModalProps) => {
  return (
    <Modal open={open} onOpenChange={onOpenChange} title={title}>
      <div className="flex flex-col items-center text-center">
        <div className="bg-red-100 p-3 rounded-full mb-4">
          <AlertTriangle className="h-8 w-8 text-brand-danger" />
        </div>
        <p className="text-gray-600 mb-6">{description}</p>
        <div className="flex w-full space-x-4">
          <Button
            variant="secondary"
            onClick={() => onOpenChange(false)}
            disabled={isConfirming}
          >
            Cancelar
          </Button>
          <Button
            variant="danger"
            onClick={onConfirm}
            isLoading={isConfirming}
          >
            Confirmar
          </Button>
        </div>
      </div>
    </Modal>
  );
};

export { ConfirmationModal };
