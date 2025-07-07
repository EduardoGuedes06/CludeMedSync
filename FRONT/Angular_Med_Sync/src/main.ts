import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router'; 
import { provideHttpClient, HTTP_INTERCEPTORS } from '@angular/common/http'; // importou provideHttpClient
import { App } from './app/app';
import { routes } from './app/app.routes';
import { JwtInterceptor } from './app/services/jwt.interceptor';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ]
}).catch(err => console.error(err));
