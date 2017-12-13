import { createStore, combineReducers, applyMiddleware } from "redux";
import { DeviceState } from "../Reducers/DeviceReducer";
import { LogState } from "../Reducers/LogReducer";
import devices from "../Reducers/DeviceReducer";
import logs from "../Reducers/LogReducer";
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
    return createStore<ApplicationState>(
        rootReducer,
        initialState,
        applyMiddleware(thunk)
    );
}