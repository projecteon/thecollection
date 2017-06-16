import * as TeaBags from '../reducers/teabags';
import * as Teabag from '../reducers/teabag';

export interface IApplicationState {
    teabags: TeaBags.ITeabagsState;
    teabag: Teabag.ITeabagState;
}

export const reducers = {
    teabags: TeaBags.reducer,
    teabag: Teabag.reducer,
};

// this type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => IApplicationState): void;
}
