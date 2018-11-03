import { IRefValue } from '../interfaces/IRefValue';
import { ITeabag } from '../interfaces/tea/IBag';

export type RefValuePropertyNames<T> = { [K in keyof T]: T[K] extends IRefValue ? K : never }[keyof T];
export type StringValuePropertyNames<T> = { [K in keyof T]: T[K] extends string ? K : never }[keyof T];

// tslint:disable-next-line:variable-name
export const EmptyRefRecord = {id: '', name: '', canaddnew: true};
// tslint:disable-next-line:variable-name
export const EmptyTeabagRecord: ITeabag = {id: '', brand: EmptyRefRecord, country: EmptyRefRecord, flavour: '', hallmark: '', imageId: '', serialNumber: '', serie: '', bagType: EmptyRefRecord, iseditable: true};
