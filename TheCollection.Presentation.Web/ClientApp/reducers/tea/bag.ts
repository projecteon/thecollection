import produce from 'immer';
import { Reducer } from 'redux';
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
  teabag: ITeabag;
  isLoading: boolean;
}

export const actionCreators = {...changeBagtype, ...changeBrand, ...changeCountry, ...clearBagtype, ...clearBrand, ...clearCountry, ...changeFlavour, ...changeHallmark, ...changeSerialNumber, ...changeSerie, ...requestTeabag, ...saveTeabag};

const emptyRef = {id: '', name: '', canaddnew: true};
const emptyTeabag: ITeabag = {id: '', brand: emptyRef, country: emptyRef, flavour: '', hallmark: '', imageId: '', serialNumber: '', serie: '', bagtype: emptyRef, iseditable: true};
const unloadedState: ITeabagState = { isLoading: false, teabag: {} as ITeabag };
type KnownActions = ChangeBagTypeAction | ChangeBrandAction | ChangeCountryAction | ClearBagTypeAction | ClearBrandAction | ClearCountryAction | ChangeFlavourAction | ChangeHallmarkAction | ChangeSerialNumberAction | ChangeSerieAction | ReceiveTeabagAction | RequestTeabagAction | SaveTeabag;
export const reducer: Reducer<ITeabagState, KnownActions> = (state = unloadedState, action) =>
  produce(state, draft => {
    switch (action.type) {
      case REQUEST_TEABAG:
        draft.teabag = emptyTeabag;
        draft.isLoading = true;
        break;
      case RECEIVE_TEABAG:
        draft.teabag = action.teabag;
        draft.isLoading = false;
        break;
      case CHANGE_BRAND:
        draft.teabag.brand = action.brand;
        break;
      case CLEAR_BRAND:
        draft.teabag.brand = emptyRef;
        break;
      case CHANGE_BAGTYPE:
        draft.teabag.bagtype = action.bagtype;
        break;
      case CLEAR_BAGTYPE:
        draft.teabag.bagtype = emptyRef;
        break;
      case CHANGE_COUNTRY:
        draft.teabag.country = action.country;
        break;
      case CLEAR_COUNTRY:
        draft.teabag.country = emptyRef;
        break;
      case CHANGE_FLAVOUR:
        draft.teabag.flavour = action.flavour;
        break;
      case CHANGE_HALLMARK:
        draft.teabag.hallmark = action.hallmark;
        break;
      case CHANGE_SERIALNUMBER:
        draft.teabag.serialNumber = action.serialnumber;
        break;
      case CHANGE_SERIE:
        draft.teabag.serie = action.serie;
        break;
      case SAVE_TEABAG:
        draft.isLoading = true;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
