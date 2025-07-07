// --- src/components/ui/Modal.tsx ---
import React from 'react';
import * as Dialog from '@radix-ui/react-dialog';
import { X } from 'lucide-react';

interface ModalProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  title: string;
  children: React.ReactNode;
}

const Modal = ({ open, onOpenChange, title, children }: ModalProps) => {
  return (
    <Dialog.Root open={open} onOpenChange={onOpenChange}>
      <Dialog.Portal>
        <Dialog.Overlay className="bg-black/50 data-[state=open]:animate-overlayShow fixed inset-0 z-40" />
        <Dialog.Content className="data-[state=open]:animate-contentShow fixed top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 rounded-lg bg-white p-6 shadow-lg z-50 w-[90vw] max-w-md max-h-[85vh] focus:outline-none">
          <Dialog.Title className="text-xl font-semibold text-gray-800 m-0 mb-4">
            {title}
          </Dialog.Title>
          
          <div>{children}</div>

          <Dialog.Close asChild>
            <button
              className="text-gray-500 hover:text-gray-700 absolute top-4 right-4 inline-flex h-6 w-6 appearance-none items-center justify-center rounded-full focus:outline-none focus:ring-2 focus:ring-brand-primary"
              aria-label="Fechar"
            >
              <X />
            </button>
          </Dialog.Close>
        </Dialog.Content>
      </Dialog.Portal>
    </Dialog.Root>
  );
};

// Precisamos adicionar as animações no nosso tailwind.config.js
// @keyframes overlayShow {
//   from { opacity: 0; }
//   to { opacity: 1; }
// }
// @keyframes contentShow {
//   from { opacity: 0; transform: translate(-50%, -48%) scale(0.96); }
//   to { opacity: 1; transform: translate(-50%, -50%) scale(1); }
// }

export { Modal };
