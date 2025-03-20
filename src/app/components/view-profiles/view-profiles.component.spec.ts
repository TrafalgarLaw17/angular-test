import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewProfilesComponent } from './view-profiles.component';

describe('ViewProfilesComponent', () => {
  let component: ViewProfilesComponent;
  let fixture: ComponentFixture<ViewProfilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewProfilesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
