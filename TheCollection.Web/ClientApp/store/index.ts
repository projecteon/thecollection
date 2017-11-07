import * as TeaBags from '../reducers/teabags';
import * as Teabag from '../reducers/teabag';
import * as Brand from '../reducers/brand';
import * as Bagtype from '../reducers/bagtype';
import * as Country from '../reducers/country';
import * as TeaDashboard from '../reducers/tea/dashboard';

export interface IApplicationState {
    teabags: TeaBags.ITeabagsState;
    teabag: Teabag.ITeabagState;
    brand: Brand.IBrandState;
    bagtype: Bagtype.IBagTypeState;
    country: Country.ICountryState;
    teadashboard: TeaDashboard.IDashboardState;
}

export const reducers = {
    teabags: TeaBags.reducer,
    teabag: Teabag.reducer,
    brand: Brand.reducer,
    bagtype: Bagtype.reducer,
    country: Country.reducer,
    teadashboard: TeaDashboard.reducer,
};

// this type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => IApplicationState): void | AppThunkAction<TAction>;
}
