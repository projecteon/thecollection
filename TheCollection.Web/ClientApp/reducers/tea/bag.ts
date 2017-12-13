import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from '../../store';
import { ITeabag } from '../../interfaces/tea/IBag';
import {
    CHANGE_BAGTYPE,
    CHANGE_BRAND,
    CHANGE_COUNTRY,
    CHANGE_FLAVOUR,
    CHANGE_HALLMARK,
    CHANGE_SERIALNUMBER,
    CHANGE_SERIE,
    CLEAR_BAGTYPE,
    CLEAR_BRAND,
    CLEAR_COUNTRY,
    RECEIVE_TEABAG,
    REQUEST_TEABAG,
    SAVE_TEABAG,
} from '../../constants/tea/bag';
import { ChangeBagTypeAction, ChangeBrandAction, ChangeCountryAction, ClearBagTypeAction, ClearBrandAction, ClearCountryAction, ChangeFlavourAction, ChangeHallmarkAction, ChangeSerialNumberAction, ChangeSerieAction, ReceiveTeabagAction, RequestTeabagAction, SaveTeabag } from '../../actions/tea/bag';
import { changeBagtype, changeBrand, changeCountry, clearBagtype, clearBrand, clearCountry, changeFlavour, changeHallmark, changeSerialNumber, changeSerie, requestTeabag, saveTeabag } from '../../thunks/tea/bag';

export interface ITeabagState {
  teabag?: ITeabag;
  isLoading: boolean;
}

export const actionCreators = {...changeBagtype, ...changeBrand, ...changeCountry, ...clearBagtype, ...clearBrand, ...clearCountry, ...changeFlavour, ...changeHallmark, ...changeSerialNumber, ...changeSerie, ...requestTeabag, ...saveTeabag};

const emptyRef = {id: '', name: '', canaddnew: true};
const emptyTeabag: ITeabag = {id: '', brand: emptyRef, country: emptyRef, flavour: '', hallmark: '', imageid: '', serialnumber: '', serie: '', bagtype: emptyRef, iseditable: true};
const unloadedState: ITeabagState = { isLoading: false, teabag: {} as ITeabag };
type KnownActions = ChangeBagTypeAction | ChangeBrandAction | ChangeCountryAction | ClearBagTypeAction | ClearBrandAction | ClearCountryAction | ChangeFlavourAction | ChangeHallmarkAction | ChangeSerialNumberAction | ChangeSerieAction | ReceiveTeabagAction | RequestTeabagAction | SaveTeabag;
export const reducer: Reducer<ITeabagState> = (state: ITeabagState, action: KnownActions) => {
  switch (action.type) {
    case REQUEST_TEABAG:
      return  {...state, ...{ teabag:  {} as ITeabag, isLoading: true }};
    case RECEIVE_TEABAG:
      return  {...state, ...{ teabag: action.teabag, isLoading: false }};
    case CHANGE_BRAND:
      return {...state, ...{teabag: {...state.teabag, ...{brand: action.brand}}}};
    case CLEAR_BRAND:
      return {...state, ...{teabag: {...state.teabag, ...{brand: undefined}}}};
    case CHANGE_BAGTYPE:
      return {...state, ...{teabag: {...state.teabag, ...{bagtype: action.bagtype}}}};
    case CLEAR_BAGTYPE:
      return {...state, ...{teabag: {...state.teabag, ...{bagtype: undefined}}}};
    case CHANGE_COUNTRY:
      return {...state, ...{teabag: {...state.teabag, ...{country: action.country}}}};
    case CLEAR_COUNTRY:
      return {...state, ...{teabag: {...state.teabag, ...{country: undefined}}}};
    case CHANGE_FLAVOUR:
      return {...state, ...{teabag: {...state.teabag, ...{flavour: action.flavour}}}};
    case CHANGE_HALLMARK:
      return {...state, ...{teabag: {...state.teabag, ...{hallmark: action.hallmark}}}};
    case CHANGE_SERIALNUMBER:
      return {...state, ...{teabag: {...state.teabag, ...{serialnumber: action.serialnumber}}}};
    case CHANGE_SERIE:
      return {...state, ...{teabag: {...state.teabag, ...{serie: action.serie}}}};
    case SAVE_TEABAG:
      return {...state, ...{isLoading: true}};
    default:
      const exhaustiveCheck: never = action;
  }

  return state || unloadedState;
};