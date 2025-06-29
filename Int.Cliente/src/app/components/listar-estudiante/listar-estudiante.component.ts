import { AfterViewInit, Component, inject, ViewChild } from '@angular/core';

import {MatTableDataSource, MatTableModule} from '@angular/material/table';
import { agreditestudiante } from 'src/app/interfaces/agreditestudiante';
import {MatPaginator, MatPaginatorModule} from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EstudianteService } from 'src/app/services/estudiante.service';

@Component({
  selector: 'app-listar-estudiante',
  templateUrl: './listar-estudiante.component.html',
  styleUrls: ['./listar-estudiante.component.css']
})
export class ListarEstudianteComponent implements AfterViewInit {

displayedColumns: string[] = ['nombre', 'email', 'accion'];

  dataSource = new MatTableDataSource<agreditestudiante>();

   @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

   ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    if(this.dataSource.data.length>0){
    this.paginator._intl.itemsPerPageLabel="Items por página";}
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  constructor(private _snackBar:MatSnackBar , private _estudsinteService:EstudianteService){}
  
  eliminarestudiante(id:number){
    this.loading=true;
    this._estudsinteService.deleteestudiante(id).subscribe(()=>{
    this.msnexito();
    this.loading=false;  
    this.ontenerEstudiante();
    })
    }
  loading :boolean=false;

  
  ontenerEstudiante(){
    this.loading=true;
  this._estudsinteService.getAllEstudiantes().subscribe({
  next:(data)=>{
    this.loading=false;
    this.dataSource.data = data.resultado }
  ,error:(e)=> {this.loading=false;}
  ,complete:() =>{
      console.info('completado')
  }
})
}

  ngOnInit():void{
    this.ontenerEstudiante();
  }


msnexito(){
  this._snackBar.open("El estudiante se Elimino correctamente",'', {
    duration:2000, horizontalPosition:'right'
  })
}

}
