import { ComboDto } from './ComboDto'; // ajusta la ruta si es necesario

export interface MateriasEstudianteDto {
  seleccionadas: ComboDto[];
  faltantes: ComboDto[];
}