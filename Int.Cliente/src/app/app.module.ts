import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AgregarEditarEstudianteComponent } from './components/agregar-editar-estudiante/agregar-editar-estudiante.component';
import { ListarEstudianteComponent } from './components/listar-estudiante/listar-estudiante.component';
import { VerCompanerosComponent } from './components/ver-companeros/ver-companeros.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

//modulos personalizados
import { SharedModule } from './Shared/shared.module';
import { AgregarMateriasComponent } from './components/agregar-materias/agregar-materias.component';


@NgModule({
  declarations: [
    AppComponent,
    AgregarEditarEstudianteComponent,
    ListarEstudianteComponent,
    VerCompanerosComponent,
    AgregarMateriasComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
