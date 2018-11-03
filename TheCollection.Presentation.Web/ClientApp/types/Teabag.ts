import { RefValue } from './RefValue';

export type Teabag = {
  id: string;
  brand: RefValue;
  serie: string;
  flavour: string;
  hallmark: string;
  type: RefValue;
  country: RefValue;
  serialnumber: string;
  imageid: string;
};
