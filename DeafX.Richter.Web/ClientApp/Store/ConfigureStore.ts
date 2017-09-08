import { createStore, combineReducers, applyMiddleware } from "redux";
import { DeviceState } from "../Reducers/DeviceReducer";
import devices from "../Reducers/DeviceReducer";
import thunk from "redux-thunk";

export interface ApplicationState {
    devices: DeviceState
}

const rootReducer = combineReducers<ApplicationState>({
    devices: devices
})

export default function configureStore(initialState: ApplicationState)
{
    return createStore<ApplicationState>(
        rootReducer,
        initialState,
        applyMiddleware(thunk)
    );
}