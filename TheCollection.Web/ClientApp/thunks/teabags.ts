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

    dispatch({ type: Constants.REQUEST_TEABAG, teabagid: teabagid });

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

export const changeSerie = {
  changeSerie: (serie: string): AppThunkAction<Actions.ChangeSerieAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_SERIE, serie: serie});
  },
};

export const changeFlavour = {
  changeFlavour: (flavour: string): AppThunkAction<Actions.ChangeFlavourAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_FLAVOUR, flavour: flavour});
  },
};

export const changeHallmark = {
  changeHallmark: (hallmark: string): AppThunkAction<Actions.ChangeHallmarkAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_HALLMARK, hallmark: hallmark});
  },
};

export const changeSerialNumber = {
  changeSerialNumber: (serialnumber: string): AppThunkAction<Actions.ChangeSerialNumberAction> => (dispatch, getState) => {
    dispatch({type: Constants.CHANGE_SERIALNUMBER, serialnumber: serialnumber});
  },
};

export const requestTeabags = {
  requestTeabags: (searchTerms?: string): AppThunkAction<Actions.RequestTeabagsAction | Actions.ReceiveTeabagsAction | Actions.SearchTermsError> => (dispatch, getState) => {
    if (searchTerms.trim().length < 3) {
      dispatch({ type: Constants.SEARCH_TERMS_ERROR, searchError: 'Requires at least 3 characters to search' });
      return;
    }

    dispatch({ type: Constants.REQUEST_TEABAGS, searchTerms: searchTerms });
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

export const saveTeabag = {
  saveTeabag: (): AppThunkAction<Actions.SaveTeabag | Actions.ReceiveTeabagAction> => (dispatch, getState) => {
    let teabag = getState().teabag.teabag;
    let method = teabag.id === undefined || teabag.id.length > 0 ? 'put' : 'post';
    try {
    let addTeabag = fetch(`/api/Bags/`, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      method: method,
      body: JSON.stringify(teabag),
    })
    .then(response => {
      dispatch({ type: Constants.RECEIVE_TEABAG, teabag: {} as ITeabag});
      addTask(addTeabag); // ensure server-side prerendering waits for this to complete
    });
    } catch (err) {
      console.log(err);
    }

    dispatch({ type: Constants.SAVE_TEABAG});
  },
};
