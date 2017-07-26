import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../store';
import { ITeabag } from '../interfaces/ITeaBag';
import { IBrand } from '../interfaces/IBrand';
import { ICountry } from '../interfaces/ICountry';
import { IBagType } from '../interfaces/IBagType';
import { ISearchResult } from '../interfaces/ISearchResult';
import * as Constants from '../constants/teabags';
import * as Actions from '../actions/teabags';

export const requestTeabag = {
  requestTeabag: (teabagid?: string): AppThunkAction<Actions.ReceiveTeabagAction | Actions.RequestTeabagAction> => (dispatch, getState) => {
    if (teabagid === undefined) {
      dispatch({ type: Constants.RECEIVE_TEABAG, teabag: {} as ITeabag });
      return;
    }

    try {
      let fetchTask = fetch(`/api/Bags/${teabagid}`)
        .then(response => response.json() as Promise<ITeabag>)
        .then(data => {
          dispatch({ type: Constants.RECEIVE_TEABAG, teabag: data });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: Constants.RECEIVE_TEABAG, teabag: {} as ITeabag });
    }

    dispatch({ type: Constants.REQUEST_TEABAG, teabagid: teabagid });
  },
};

export const changeBrand = {
  changeBrand: (brand: IBrand): AppThunkAction<Actions.ChangeBrandAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_BRAND, brand: brand});
  },
};

export const clearBrand = {
 clearBrand: (): AppThunkAction<Actions.ClearBrandAction> => (dispatch, getState) => {
    dispatch({type: Constants.CLEAR_BRAND});
  },
};

export const changeBagtype = {
  changeBagtype: (bagtype: IBagType): AppThunkAction<Actions.ChangeBagTypeAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_BAGTYPE, bagtype: bagtype});
  },
};

export const clearBagtype = {
  clearBagtype: (): AppThunkAction<Actions.ClearBagTypeAction> => (dispatch, getState) => {
    dispatch({type: Constants.CLEAR_BAGTYPE});
  },
};

export const changeCountry = {
  changeCountry: (country: ICountry): AppThunkAction<Actions.ChangeCountryAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_COUNTRY, country: country});
  },
};

export const clearCountry = {
  clearCountry: (): AppThunkAction<Actions.ClearCountryAction> => (dispatch, getState) => {
    dispatch({type: Constants.CLEAR_COUNTRY});
  },
};

export const requestTeabags = {
  requestTeabags: (searchTerms?: string): AppThunkAction<Actions.RequestTeabagsAction | Actions.ReceiveTeabagsAction | Actions.SearchTermsError> => (dispatch, getState) => {
    if (searchTerms.trim().length < 3) {
      dispatch({ type: Constants.SEARCH_TERMS_ERROR, searchError: 'Requires at least 3 characters to search' });
      return;
    }

    let uri = searchTerms !== undefined && searchTerms.length > 0 ? `/api/Bags/?searchterm=${encodeURIComponent(searchTerms)}` : `/api/Bags/`;
    try {
      let fetchTask = fetch(uri)
        .then(response => response.json() as Promise<ISearchResult<ITeabag>>)
        .then(data => {
          dispatch({ type: Constants.RECEIVE_TEABAGS, searchTerms: searchTerms, teabags: data.data, resultCount: data.count });
          addTask(fetchTask); // ensure server-side prerendering waits for this to complete
      });
    } catch (err) {
      dispatch({ type: Constants.RECEIVE_TEABAGS, searchTerms: searchTerms, teabags: [], resultCount: 0 });
    }

    dispatch({ type: Constants.REQUEST_TEABAGS, searchTerms: searchTerms });
  },
};

export const validateSearchTerms = {
  validateSearchTerms: (searchTerms: string): AppThunkAction<Actions.SearchTermsError> => (dispatch, getState) => {
    if (getState().teabags.searchError !== '' && searchTerms.trim().length > 2) {
      dispatch({ type: Constants.SEARCH_TERMS_ERROR, searchError: '' });
    }
  },
};

export const zoomImage = {
  zoomImage: (imageid: string): AppThunkAction<Actions.ZoomImage> => (dispatch, getState) => {
    dispatch({ type: Constants.ZOOM_IMAGE_TOGGLE, imageid: imageid });
  },
};
