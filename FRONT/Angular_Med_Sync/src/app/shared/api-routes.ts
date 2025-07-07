export class ApiRoutes {
  static readonly login = 'api/auth/login';
  static readonly register = 'api/auth/register';

  static readonly funcionarios = 'funcionarios';
  static readonly consultas = 'consultas';
  static readonly pacientes = 'pacientes';
  static fullPath(route: string, baseUrl: string): string {
    return `${baseUrl}/${route}`;
  }
}