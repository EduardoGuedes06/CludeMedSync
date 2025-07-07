export interface User {
  id: string;
  name: string;
  email: string;
}

export interface AuthContextType {
  isAuthenticated: boolean;
  user: User | null;
  signIn: (credentials: { email: string; password: string }) => Promise<void>;
  signOut: () => void;
  loading: boolean;
}