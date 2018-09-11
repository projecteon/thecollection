import produce from 'immer';
import { Reducer } from 'redux';
import { ITeabag } from '../../interfaces/tea/IBag';
import { BagActionTypes, ChangeBagTypeAction, ChangeBrandAction, ChangeCountryAction, ClearBagTypeAction, ClearBrandAction, ClearCountryAction, ChangeFlavourAction, ChangeHallmarkAction, ChangeSerialNumberAction, ChangeSerieAction, ReceiveTeabagAction, RequestTeabagAction, SaveTeabag } from '../../actions/tea/bag';
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
      case BagActionTypes.RequestBag:
        draft.teabag = emptyTeabag;
        draft.isLoading = true;
        break;
      case BagActionTypes.RecieveBag:
        draft.teabag = action.teabag;
        draft.isLoading = false;
        break;
      case BagActionTypes.ChangeBrand:
        draft.teabag.brand = action.brand;
        break;
      case BagActionTypes.ClearBrand:
        draft.teabag.brand = emptyRef;
        break;
      case BagActionTypes.ChangeBagType:
        draft.teabag.bagtype = action.bagtype;
        break;
      case BagActionTypes.ClearBagType:
        draft.teabag.bagtype = emptyRef;
        break;
      case BagActionTypes.ChangeCountry:
        draft.teabag.country = action.country;
        break;
      case BagActionTypes.ClearCountry:
        draft.teabag.country = emptyRef;
        break;
      case BagActionTypes.ChangeFlavor:
        draft.teabag.flavour = action.flavour;
        break;
      case BagActionTypes.ChangeHallmark:
        draft.teabag.hallmark = action.hallmark;
        break;
      case BagActionTypes.ChangeSerialNumber:
        draft.teabag.serialNumber = action.serialnumber;
        break;
      case BagActionTypes.ChangeSerie:
        draft.teabag.serie = action.serie;
        break;
      case BagActionTypes.Save:
        draft.isLoading = true;
        break;
      default:
        const exhaustiveCheck: never = action;
    }
  });
