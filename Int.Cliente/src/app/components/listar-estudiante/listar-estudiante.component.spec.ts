import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListarEstudianteComponent } from './listar-estudiante.component';

describe('ListarEstudianteComponent', () => {
  let component: ListarEstudianteComponent;
  let fixture: ComponentFixture<ListarEstudianteComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ListarEstudianteComponent]
    });
    fixture = TestBed.createComponent(ListarEstudianteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
