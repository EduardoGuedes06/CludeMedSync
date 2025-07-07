import { TestBed } from '@angular/core/testing';

import { Profissionais } from './profissionais';

describe('Profissionais', () => {
  let service: Profissionais;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Profissionais);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
