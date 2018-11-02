import { IRefValue } from '../IRefValue';

export enum PropNames {
  id = 'id',
  brand = 'brand',
  serie = 'serie',
  flavour = 'flavour',
  hallmark = 'hallmark',
  bagType = 'bagType',
  country = 'country',
  serialNumber = 'serialNumber',
  imageId = 'imageId',
  iseditable = 'iseditable',
}

export interface ITeabag {
  [PropNames.id]: string;
  [PropNames.brand]: IRefValue;
  [PropNames.serie]: string;
  [PropNames.flavour]: string;
  [PropNames.hallmark]: string;
  [PropNames.bagType]: IRefValue;
  [PropNames.country]: IRefValue;
  [PropNames.serialNumber]: string;
  [PropNames.imageId]: string;
  [PropNames.iseditable]: boolean;
}
