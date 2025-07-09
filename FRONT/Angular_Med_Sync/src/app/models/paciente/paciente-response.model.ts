export interface PacienteResponse {
  id: number;
  nomeCompleto: string;
  dataNascimento: Date;
  cpf: string;
  email: string;
  telefone?: string;
  ativo: boolean;
}
