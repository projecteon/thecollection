import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import { ITeabag } from '../interfaces/ITeaBag';
import { IBrand } from '../interfaces/IBrand';
import { ICountry } from '../interfaces/ICountry';
import { IBagType } from '../interfaces/IBagType';

export interface ITeabagState {
  teabag?: ITeabag;
  isLoading: boolean;
  searchedBagTypes: IBagType[];
  searchedCountries: ICountry[];
}

interface IRequestTeabag {
  type: 'REQUEST_TEABAG';
  teabagid: string;
}

interface IReceiveTeabag {
  type: 'RECEIVE_TEABAG';
  teabag: ITeabag;
}

interface IChangeBrand {
  type: 'CHANGE_BRAND';
  brand: IBrand;
}

interface IClearBrand {
  type: 'CLEAR_BRAND';
}

interface IChangeBagtype {
  type: 'CHANGE_BAGTYPE';
  bagtype: IBagType;
}

interface IClearBagType {
  type: 'CLEAR_BAGTYPE';
}

interface IChangeCountry {
  type: 'CHANGE_COUNTRY';
  country: ICountry;
}

interface IClearCountry {
  type: 'CLEAR_COUNTRY';
}

export const actionCreators = {
  requestTeabag: (teabagid?: string): AppThunkAction<IReceiveTeabag | IRequestTeabag> => (dispatch, getState) => {
    if (teabagid === undefined) {
      dispatch({ type: 'RECEIVE_TEABAG', teabag: {} as ITeabag });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Bags/${teabagid}`)
        .then(response => response.json() as Promise<ITeabag>)
        .then(data => {
          dispatch({ type: 'RECEIVE_TEABAG', teabag: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: 'RECEIVE_TEABAG', teabag: {} as ITeabag });
    }

    dispatch({ type: 'REQUEST_TEABAG', teabagid: teabagid });
  },
  changeBrand: (brand: IBrand): AppThunkAction<IChangeBrand> => (dispatch, getState) => {
    dispatch({type: 'CHANGE_BRAND', brand: brand});
  },
  clearBrand: (): AppThunkAction<IClearBrand> => (dispatch, getState) => {
    dispatch({type: 'CLEAR_BRAND'});
  },
  changeBagtype: (bagtype: IBagType): AppThunkAction<IChangeBagtype> => (dispatch, getState) => {
    dispatch({type: 'CHANGE_BAGTYPE', bagtype: bagtype});
  },
  clearBagtype: (): AppThunkAction<IClearBagType> => (dispatch, getState) => {
    dispatch({type: 'CLEAR_BAGTYPE'});
  },
  changeCountry: (country: ICountry): AppThunkAction<IChangeCountry> => (dispatch, getState) => {
    dispatch({type: 'CHANGE_COUNTRY', country: country});
  },
  clearCountry: (): AppThunkAction<IClearCountry> => (dispatch, getState) => {
    dispatch({type: 'CLEAR_COUNTRY'});
  },
};
const unloadedState: ITeabagState = { isLoading: false, teabag: {} as ITeabag, searchedBagTypes: [], searchedCountries: [] };

type TeabagActions = IReceiveTeabag | IRequestTeabag | IChangeBrand | IClearBrand | IChangeBagtype | IClearBagType | IChangeCountry | IClearCountry;
export const reducer: Reducer<ITeabagState> = (state: ITeabagState, action: TeabagActions) => {
  switch (action.type) {
    case 'REQUEST_TEABAG':
      return  {...state, ...{ teabag: {} as ITeabag, isLoading: true }};
    case 'RECEIVE_TEABAG':
      return  {...state, ...{ teabag: action.teabag, isLoading: false }};
    case 'CHANGE_BRAND':
      return {...state, ...{teabag: {...state.teabag, ...{brand: action.brand}}}};
    case 'CLEAR_BRAND':
      return {...state, ...{teabag: {...state.teabag, ...{brand: undefined}}}};
    case 'CHANGE_BAGTYPE':
      return {...state, ...{teabag: {...state.teabag, ...{type: action.bagtype}}}};
    case 'CLEAR_BAGTYPE':
      return {...state, ...{teabag: {...state.teabag, ...{type: undefined}}}};
    case 'CHANGE_COUNTRY':
      return {...state, ...{teabag: {...state.teabag, ...{country: action.country}}}};
    case 'CLEAR_COUNTRY':
      return {...state, ...{teabag: {...state.teabag, ...{country: undefined}}}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};

