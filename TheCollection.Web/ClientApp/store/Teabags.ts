import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface TeabagsState {
    isLoading: boolean;
    startDateIndex: number;
    teabags: Teabag[];
}

export interface Teabag {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestTeabagsAction {
    type: 'REQUEST_TEABAGS',
    startDateIndex: number;
}

interface ReceiveTeabagsAction {
    type: 'RECEIVE_TEABAGS',
    startDateIndex: number;
    teabags: Teabag[]
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestTeabagsAction | ReceiveTeabagsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestTeabags: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        if (startDateIndex !== getState().teabags.startDateIndex) {
            let fetchTask = fetch(`/api/Bags/`)
                .then(response => response.json() as Promise<Teabag[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_TEABAGS', startDateIndex: startDateIndex, teabags: data });
                });

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
            dispatch({ type: 'REQUEST_TEABAGS', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: TeabagsState = { startDateIndex: null, teabags: [], isLoading: false };

export const reducer: Reducer<TeabagsState> = (state: TeabagsState, action: KnownAction) => {
    switch (action.type) {
        case 'REQUEST_TEABAGS':
            return {
                startDateIndex: action.startDateIndex,
                teabags: state.teabags,
                isLoading: true
            };
        case 'RECEIVE_TEABAGS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    teabags: action.teabags,
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
