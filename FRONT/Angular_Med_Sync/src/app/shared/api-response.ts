export interface ApiResponse<T> {
  sucesso: boolean;
  mensagem: string;
  status: number;
  dados: T;
}
