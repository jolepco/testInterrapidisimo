import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerCompanerosComponent } from './ver-companeros.component';

describe('VerCompanerosComponent', () => {
  let component: VerCompanerosComponent;
  let fixture: ComponentFixture<VerCompanerosComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [VerCompanerosComponent]
    });
    fixture = TestBed.createComponent(VerCompanerosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
