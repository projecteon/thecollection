import { combineReducers } from 'redux';
import * as SearchTeaBags from '../reducers/tea/search';
import * as Teabag from '../reducers/tea/bag';
import * as Brand from '../reducers/tea/brand';
import * as Bagtype from '../reducers/tea/bagtype';
import * as Country from '../reducers/country';
import * as TeaDashboard from '../reducers/tea/dashboard';
import * as UI from '../reducers/ui';

export interface IApplicationState {
  searchteabag: SearchTeaBags.ISearchState;
  teabag: Teabag.ITeabagState;
  brand: Brand.IBrandState;
  bagtype: Bagtype.IBagTypeState;
  country: Country.ICountryState;
  teadashboard: TeaDashboard.IDashboardState;
  ui: UI.IUIState;
}

export const reducers = {
  searchteabag: SearchTeaBags.reducer,
  teabag: Teabag.reducer,
  brand: Brand.reducer,
  bagtype: Bagtype.reducer,
  country: Country.reducer,
  teadashboard: TeaDashboard.reducer,
  ui: UI.reducer,
};

// this type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
// tslint:disable-next-line:interface-name
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => IApplicationState): void | AppThunkAction<TAction>;
}
