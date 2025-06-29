import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { vercompaneros } from 'src/app/interfaces/vercompaneros';
import { EstudianteService } from 'src/app/services/estudiante.service';

@Component({
  selector: 'app-ver-companeros',
  templateUrl: './ver-companeros.component.html',
  styleUrls: ['./ver-companeros.component.css']
})

export class VerCompanerosComponent  implements AfterViewInit{
displayedColumns: string[] = ['materia', 'nombreEstudiante'];

dataSource = new MatTableDataSource<vercompaneros>();
id!:number;
   @ViewChild(MatPaginator) paginator!: MatPaginator;
    @ViewChild(MatSort) sort!: MatSort;

   ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    if(this.dataSource.data.length>0){
    this.paginator._intl.itemsPerPageLabel="Items por página";
    }
  }
loading :boolean=false;
  constructor(private _estudianteService:EstudianteService
    , private aRoute: ActivatedRoute
  ){
     //this.id=Number(this.aRoute.snapshot.paramMap.get('id'))
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  ngOnInit(){
  //this.obtenercompanero()   
  this.aRoute.params.subscribe(data=>{
    this.id = data["id"];
    this.obtenercompanero();
  })
  }
  obtenercompanero(){
    this._estudianteService.getCompaneros(this.id).subscribe({
  next:(data)=>{
    debugger;
    console.log(data.resultado)
    this.loading=false;
    this.dataSource.data = data.resultado }
  ,error:(e)=> {this.loading=false;}
  ,complete:() =>{
      console.info('completado')
  }
  })
}
}
