import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';
import {ITeabag} from '../interfaces/ITeaBag';
import { ISearchResult } from "../interfaces/ISearchResult";

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TeabagsState {
  teabags: ITeabag[];
  resultCount?: number;
  isLoading: boolean;
  zoomUri?: string;
  searchTerms?: string;
  searchError?: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTeabagsAction {
    type: 'REQUEST_TEABAGS';
    searchTerms: string;
}

interface ReceiveTeabagsAction {
    type: 'RECEIVE_TEABAGS';
    searchTerms: string;
    teabags: ITeabag[];
    resultCount: number;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTeabagsAction | ReceiveTeabagsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTeabags: (searchTerms?: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        if (searchTerms !== getState().teabags.searchTerms) {
            let uri = searchTerms !== undefined && searchTerms.length > 0
              ? `/api/Bags/?searchterm=${encodeURIComponent(searchTerms)}`
              : `/api/Bags/`;
            let fetchTask = fetch(uri)
              .then(response => response.json() as Promise<ISearchResult<ITeabag>>)
              .then(data => {
                console.log(`loaded ${data.count} rows`);
                dispatch({ type: 'RECEIVE_TEABAGS', searchTerms: searchTerms, teabags: data.data, resultCount: data.count });
              });

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
            dispatch({ type: 'REQUEST_TEABAGS', searchTerms: searchTerms });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TeabagsState = { isLoading: false, teabags: [], searchTerms: '' };

export const reducer: Reducer<TeabagsState> = (state: TeabagsState, action: KnownAction) => {
    switch (action.type) {
        case 'REQUEST_TEABAGS':
            return {
                searchTerms: action.searchTerms,
                teabags: state.teabags,
                isLoading: true
            };
        case 'RECEIVE_TEABAGS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.searchTerms === state.searchTerms) {
                return {
                    searchTerms: action.searchTerms,
                    teabags: action.teabags,
                    resultCount: action.resultCount,
                    isLoading: false
                };
            }
            break;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};
