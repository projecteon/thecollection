import { ICountBy } from '../../interfaces/ICountBy';
import { IRefValue } from '../../interfaces/IRefValue';

export enum DashboardActionTypes {
  RecieveBagTypeCount = '[Dashboard] RecieveBagTypeCount',
  RequestBagTypeCount = '[Dashboard] RequestBagTypeCount',
  RecieveBrandCount = '[Dashboard] RecieveBrandCount',
  RequestBrandCount = '[Dashboard] RequestBrandCount',
}

export type ReceiveBagTypeCountAction = {
  type: DashboardActionTypes.RecieveBagTypeCount;
  data: ICountBy<IRefValue>[];
};

export type RequestBagTypeCountAction = {
  type: DashboardActionTypes.RequestBagTypeCount;
};

export type ReceiveBrandCountAction = {
  type: DashboardActionTypes.RecieveBrandCount;
  data: ICountBy<IRefValue>[];
};

export type RequestBrandCountAction = {
  type: DashboardActionTypes.RequestBrandCount;
};
