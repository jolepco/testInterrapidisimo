import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { env_dev } from 'src/environments/env_dev';
import { agreditestudiante } from '../interfaces/agreditestudiante';
import { Respuesta } from '../interfaces/respuesta';
import { vercompaneros } from '../interfaces/vercompaneros';
import { MateriasEstudianteDto } from '../interfaces/MateriasEstudianteDto';
import { envioData } from '../interfaces/EnvioData';

@Injectable({
  providedIn: 'root'
})
export class EstudianteService {
private myAppUrl = env_dev.endpoint;
  constructor(private http:HttpClient) { }

  getAllEstudiantes():Observable<Respuesta<agreditestudiante[]>>
  {
    return this.http.get<Respuesta<agreditestudiante[]>>(`${this.myAppUrl}api/Estudiante`)
  }

  getCompaneros(id: number): Observable<Respuesta<vercompaneros[]>> {
  return this.http.get<Respuesta<vercompaneros[]>>(`${this.myAppUrl}api/Estudiante/${id}`);
  }

  getestudiante(id: number): Observable<Respuesta<agreditestudiante>> {
  return this.http.get<Respuesta<agreditestudiante>>(`${this.myAppUrl}api/Estudiante/Estudianteid/${id}`);
  }

  addestudiante(AEestudiante: agreditestudiante):Observable<agreditestudiante> {
    return this.http.post<agreditestudiante>(`${this.myAppUrl}api/Estudiante/addest`,AEestudiante)
  }

  editestudiante(id:number, AEestudiante: agreditestudiante):Observable<void> {
    return this.http.put<void>(`${this.myAppUrl}api/Estudiante/${id}`,AEestudiante)
  }

deleteestudiante(id:number):Observable<void> {
    return this.http.delete<void>(`${this.myAppUrl}api/Estudiante/${id}`)
  }

  obtenermaterias(id: number): Observable<Respuesta<MateriasEstudianteDto>> {
  return this.http.get<Respuesta<MateriasEstudianteDto>>(`${this.myAppUrl}api/Estudiante/Materias/${id}`);
  }  

   agregarmaterias(id: number,  materias: envioData): Observable<Respuesta<boolean>> {
  return this.http.post<Respuesta<boolean>>(`${this.myAppUrl}api/Estudiante/actualizar/${id}`, materias);
  }  
}


  