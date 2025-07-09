export interface PacienteRequest {
  nomeCompleto: string;
  dataNascimento: Date;
  cpf: string;
  email: string;
  telefone?: string;
  ativo: boolean;
}
