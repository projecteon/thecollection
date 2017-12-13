import { IRefValue } from '../IRefValue';
import { IBrand } from './IBrand';
import { ICountry } from '../ICountry';
import { IBagType } from './IBagType';

export interface ITeabag {
  id: string;
  brand: IRefValue;
  serie: string;
  flavour: string;
  hallmark: string;
  bagtype: IRefValue;
  country: IRefValue;
  serialnumber: string;
  imageid: string;
  iseditable: boolean;
}
