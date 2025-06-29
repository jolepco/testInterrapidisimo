import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { agreditestudiante } from 'src/app/interfaces/agreditestudiante';
import { EstudianteService } from 'src/app/services/estudiante.service';

@Component({
  selector: 'app-agregar-editar-estudiante',
  templateUrl: './agregar-editar-estudiante.component.html',
  styleUrls: ['./agregar-editar-estudiante.component.css']
})
export class AgregarEditarEstudianteComponent  {
 //observable
  //nestudiante$!:Observable<agreditestudiante>
  operacion:string='Aregar';
  
  id:number;
  loading:boolean=false;
  form:FormGroup;

  constructor(private fb: FormBuilder  , private aRoute: ActivatedRoute
   , private _estudsinteService:EstudianteService, private _snackBar:MatSnackBar 
   , private _router:Router
   )
   {
    
    this.id=Number(this.aRoute.snapshot.paramMap.get('id'));
    
  this.form = this.fb.group({
    nombre:['',Validators.required],
    email:['',Validators.required],
  });
}

ngOnInit():void{

  if(this.id !=0) {
    this.operacion='Editar';
    this.obtenerestudiante(this.id);
  //this.nestudiante$ = this._estudsinteService.getestudiante(this.id);

  }
}

obtenerestudiante(id:number) {
  this.loading=true;
this._estudsinteService.getestudiante(id).subscribe(data =>{
this.loading=false;
//this.form.setValue({
this.form.patchValue({
  nombre:data.resultado.nombre, 
  email:data.resultado.email,
  id:data.resultado.id
})
console.log(data)
});
}

agregarestudiante(){
//const nombre =this.form.get('nombre')?.value;
//const nombre =this.form.value.nombre;
//armamos el objeto

const Nuevoestudinte: agreditestudiante ={
  nombre:this.form.get('nombre')?.value, 
  email:this.form.get('email')?.value, 
};


if(this.id !=0) {
Nuevoestudinte.id =this.id;
this.EditarEstudiante(this.id, Nuevoestudinte);
}else 
  {
    this.agregarNuevoEstudiante(Nuevoestudinte);
  }


}
EditarEstudiante(id:number,aeestudiante: agreditestudiante ) {
  this.loading=true;
  this._estudsinteService.editestudiante(id, aeestudiante).subscribe(()=>{
    this.loading=false;
     this.msnexito("El estudiante se actualizó correctamente!");

  })
}

agregarNuevoEstudiante(aeestudiante: agreditestudiante)
{
  this._estudsinteService.addestudiante(aeestudiante).subscribe(data=>{
    this.msnexito("El estudiante se registro correctamente!");
this._router.navigate(['/ListEstudiantes'])
})
}
msnexito(text:string){
  this._snackBar.open(text,'', {
    duration:2000, horizontalPosition:'right'
  })
}
}
