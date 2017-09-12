import { IBrand } from './IBrand';
import { ICountry } from './ICountry';
import { IBagType } from './IBagType';

export interface ITeabag {
  id: string;
  brand: IBrand;
  serie: string;
  flavour: string;
  hallmark: string;
  bagtype: IBagType;
  country: ICountry;
  serialnumber: string;
  imageid: string;
}
