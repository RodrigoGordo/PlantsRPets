import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module';

platformBrowserDynamic().bootstrapModule(AppModule, {
  ngZoneEventCoalescing: true,
})
  .catch(err => console.error(err));

navigator.serviceWorker.register('firebase-messaging-sw.js')
  .then((registration) => {
    console.log('Service Worker registrado!', registration);
  })
  .catch((error) => {
    console.error('Erro ao registrar Service Worker:', error);
  });
