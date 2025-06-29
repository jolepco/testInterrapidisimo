import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarMateriasComponent } from './agregar-materias.component';

describe('AgregarMateriasComponent', () => {
  let component: AgregarMateriasComponent;
  let fixture: ComponentFixture<AgregarMateriasComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AgregarMateriasComponent]
    });
    fixture = TestBed.createComponent(AgregarMateriasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
