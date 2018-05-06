import { createStore, combineReducers, applyMiddleware } from "redux";
import devices, { DeviceState } from "reducers/deviceReducer";
import logs, { LogState } from "reducers/logReducer";
import thunk from "redux-thunk";

export interface ApplicationState {
    devices: DeviceState
    logs: LogState
}

const rootReducer = combineReducers<ApplicationState>({
    devices: devices,
    logs: logs
})

export default function configureStore(initialState: ApplicationState)
{
//     return createStore<ApplicationState>(
//     rootReducer,
//     initialState,
//     applyMiddleware(thunk)
//      );
    return createStore(
        rootReducer,
        initialState,
        applyMiddleware(thunk)
    );
}