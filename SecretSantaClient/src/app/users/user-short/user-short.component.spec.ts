/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UserShortComponent } from './user-short.component';

describe('UserShortComponent', () => {
  let component: UserShortComponent;
  let fixture: ComponentFixture<UserShortComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserShortComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserShortComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
