/// <reference types="@angular/localize" />

import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import 'bootstrap';
import { provideRouter } from '@angular/router';
import {appRoutes} from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimations } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { importProvidersFrom } from '@angular/core';

bootstrapApplication(AppComponent,{providers:[
  provideRouter(appRoutes), provideHttpClient(),provideAnimations(),
  importProvidersFrom(NgbModule, BrowserAnimationsModule)
]})
  .catch((err) => console.error(err));
