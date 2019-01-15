import { TestBed } from '@angular/core/testing';

import { UnpaidService } from './unpaid.service';

describe('UnpaidService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UnpaidService = TestBed.get(UnpaidService);
    expect(service).toBeTruthy();
  });
});
