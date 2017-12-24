/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GroupsService } from './groups.service';

describe('Service: Groups', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GroupsService]
    });
  });

  it('should ...', inject([GroupsService], (service: GroupsService) => {
    expect(service).toBeTruthy();
  }));
});