import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListarEstudianteComponent } from './components/listar-estudiante/listar-estudiante.component';
import { AgregarEditarEstudianteComponent } from './components/agregar-editar-estudiante/agregar-editar-estudiante.component';
import { VerCompanerosComponent } from './components/ver-companeros/ver-companeros.component';
import { AgregarMateriasComponent } from './components/agregar-materias/agregar-materias.component';

const routes: Routes = [
{ path:'',redirectTo: 'ListEstudiantes', pathMatch:'full' },
{ path:'ListEstudiantes', component:ListarEstudianteComponent},
{ path:'agregarEstudiantes', component:AgregarEditarEstudianteComponent},
{ path:'editarEstudiantes/:id', component:AgregarEditarEstudianteComponent},
{ path:'verCompaneros/:id', component:VerCompanerosComponent},
{ path:'agregarMaterias/:id', component:AgregarMateriasComponent},
{ path:'**',redirectTo: 'ListEstudiantes', pathMatch:'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
