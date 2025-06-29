import { DataSource } from '@angular/cdk/collections';
import { SelectorListContext } from '@angular/compiler';
import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ComboDto } from 'src/app/interfaces/ComboDto';
import { envioData } from 'src/app/interfaces/EnvioData';
import { MateriasEstudianteDto } from 'src/app/interfaces/MateriasEstudianteDto';
import { EstudianteService } from 'src/app/services/estudiante.service';

@Component({
  selector: 'app-agregar-materias',
  templateUrl: './agregar-materias.component.html',
  styleUrls: ['./agregar-materias.component.css']
})

export class AgregarMateriasComponent {

  id!:number;
loading:boolean=false; 

izquierda!:ComboDto[]
derecha!:ComboDto[]
envioDataGeneral! : envioData;

  moverADerecha(item: ComboDto) {
    this.derecha.push(item);
    this.izquierda = this.izquierda.filter(i => i.id !== item.id);
  }

  moverAIzquierda(item: ComboDto) {
    this.izquierda.push(item);
    this.derecha = this.derecha.filter(i => i.id !== item.id);
  }


  constructor(private aRoute: ActivatedRoute, 
    private _estudianteService:EstudianteService,
    private _snackBar:MatSnackBar ,
    private _router:Router
  ){
      this.id=Number(this.aRoute.snapshot.paramMap.get('id'));
      console.log(this.id)
  }

ngOnInit(){
    this.obtenermaterias(this.id);
}

  obtenermaterias(id:number){
      this._estudianteService.obtenermaterias(id).subscribe(data =>{
      this.izquierda = data.resultado.seleccionadas;
      this.derecha = data.resultado.faltantes;

      });
  }

  guardarmateriasxaestudiante(){
  this.loading=true;
    var inscritas=this.izquierda.map(item => item.id);
    var eliminadas=this.derecha.map(item => item.id);

    this.envioDataGeneral = {
  seleccionadas:inscritas, 
  faltantes:eliminadas
}

    this._estudianteService.agregarmaterias(this.id , this.envioDataGeneral).subscribe(data =>{
     debugger;
     this.loading=false;
     if(data.resultado==true) {
      this._router.navigate(['/ListEstudiantes'])
     }
    this.msnexito(data.mensaje);
    });
  }

  puedeEnviar(): boolean {
  return this.izquierda.length === 3;
}

msnexito(text:string){
  this._snackBar.open(text,'', {
    duration:4000, horizontalPosition:'right'
  })
}

}
