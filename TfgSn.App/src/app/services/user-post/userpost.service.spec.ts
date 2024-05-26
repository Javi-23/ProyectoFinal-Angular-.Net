import { TestBed } from '@angular/core/testing';

import { UserpostService } from './userpost.service';

describe('UserpostService', () => {
  let service: UserpostService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserpostService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
