import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, catchError, throwError } from 'rxjs';

// Importando os modelos e rotas
import { environment } from '../../environments/environment';
import { ApiRoutes } from '../../shared/api-routes';
import { ApiResponse } from '../../shared/api-response';
import { PagedResult } from '../../models/generic/paged-result.model';
import { Paciente } from '../../models/paciente/paciente.model';
import { PacienteRequest } from '../../models/paciente/paciente-request.model';
import { PacienteResponse } from '../../models/paciente/paciente-response.model';

@Injectable({ providedIn: 'root' })
export class PacienteService {
  private baseUrl = environment.apiBaseUrl;
  private endpoint = ApiRoutes.pacientes;

  constructor(private http: HttpClient) {}

  listarPacientes(
    page: number = 1,
    pageSize: number = 10,
    filtros: any = {}
  ): Observable<PagedResult<Paciente>> {
    
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());


    for (const key in filtros) {
      if (filtros.hasOwnProperty(key) && filtros[key]) {
        params = params.append(key, filtros[key]);
      }
    }

    return this.http.get<ApiResponse<PagedResult<Paciente>>>(`${this.baseUrl}/${this.endpoint}`, { params })
      .pipe(
        map(response => {
          if (!response.sucesso) {
            throw new Error(response.mensagem || 'Erro ao buscar pacientes.');
          }
          return response.dados; 
        }),
        catchError(this.handleError)
      );
  }

  obterPorId(id: number): Observable<PacienteResponse> {
    return this.http.get<ApiResponse<PacienteResponse>>(`${this.baseUrl}/${this.endpoint}/${id}`)
      .pipe(
        map(response => {
          if (!response.sucesso) {
            throw new Error(response.mensagem || `Erro ao buscar paciente com ID ${id}.`);
          }
          return response.dados;
        }),
        catchError(this.handleError)
      );
  }

  criar(paciente: PacienteRequest): Observable<PacienteResponse> {
    return this.http.post<ApiResponse<PacienteResponse>>(`${this.baseUrl}/${this.endpoint}`, paciente)
      .pipe(
        map(response => {
          if (!response.sucesso) {
            throw new Error(response.mensagem || 'Erro ao criar paciente.');
          }
          return response.dados;
        }),
        catchError(this.handleError)
      );
  }

  atualizar(id: number, paciente: PacienteRequest): Observable<PacienteResponse> {
    return this.http.put<ApiResponse<PacienteResponse>>(`${this.baseUrl}/${this.endpoint}/${id}`, paciente)
      .pipe(
        map(response => {
          if (!response.sucesso) {
            throw new Error(response.mensagem || 'Erro ao atualizar paciente.');
          }
          return response.dados;
        }),
        catchError(this.handleError)
      );
  }

  excluir(id: number): Observable<any> {
    return this.http.delete<ApiResponse<any>>(`${this.baseUrl}/${this.endpoint}/${id}`)
      .pipe(
        map(response => {
          if (!response.sucesso) {
            throw new Error(response.mensagem || 'Erro ao excluir paciente.');
          }
          return response;
        }),
        catchError(this.handleError)
      );
  }

  private handleError(error: any): Observable<never> {
    const errorMsg = error.message || 'Erro inesperado do servidor.';
    console.error(`PacienteService Error: ${errorMsg}`);
    return throwError(() => new Error(errorMsg));
  }
}