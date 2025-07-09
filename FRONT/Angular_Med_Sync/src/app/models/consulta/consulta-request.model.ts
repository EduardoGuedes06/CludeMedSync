export interface ConsultaRequest {
  usuarioId: string;
  pacienteId: number;
  profissionalId: number;
  dataHoraInicio: string;
  dataHoraFim?: string;
  motivo?: string;
  observacao?: string;
  status: number;
}
