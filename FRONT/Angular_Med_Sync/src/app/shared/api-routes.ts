export class ApiRoutes {
  static readonly login = 'api/auth/login';
  static readonly register = 'api/auth/register';

  static readonly funcionarios = 'api/funcionarios';
  static readonly consultas = 'api/consultas';
  static readonly pacientes = 'api/pacientes';
  static fullPath(route: string, baseUrl: string): string {
    return `${baseUrl}/${route}`;
  }
}