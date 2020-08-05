import { TestBed } from '@angular/core/testing';

import { CashService } from './cash.service';

describe('CashServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CashService = TestBed.get(CashService);
    expect(service).toBeTruthy();
  });
});
