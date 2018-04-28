import { Reducer, Action } from "redux";
import { SetErrorMessageAction, AddClientLogAction, AddServerLogAction, } from "../Actions/LogActions";
import { ClientLog } from "../Models/Log/ClientLog";
import { ServerLog } from "../Models/Log/ServerLog";

export interface LogState {
    clientLogs: ClientLog[];
    serverLogs: ServerLog[];
    currentErrorMessage: string;
}

const logReducer: Reducer<LogState> = (state = { clientLogs: [], serverLogs: [], currentErrorMessage: null }, action: Action) => {
    switch (action.type) {
        case "ADD_SERVER_LOG":
            return addServerLog(action as AddServerLogAction, state);
        case "ADD_CLIENT_LOG":
            return addClientLog(action as AddClientLogAction, state);
        case "SET_ERROR_MSG":
            return { ...state, currentErrorMessage: (action as SetErrorMessageAction).message };
        case "REMOVE_ERROR_MSG":
            return { ...state, currentErrorMessage: null };
        default:
            return state;
    }
}

function addServerLog(action: AddServerLogAction, state: LogState): LogState {
    return {
        ...state,
        serverLogs: [...state.serverLogs, action.logItem]
    };
}

function addClientLog(action: AddClientLogAction, state: LogState): LogState {
    return {
        ...state,
        clientLogs: [...state.clientLogs, action.logItem]
    };
}

export default logReducer;