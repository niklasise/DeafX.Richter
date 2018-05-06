import { ClientLog } from "models/log/clientLog";
import { ServerLog } from "models/log/serverLog";
import { Action } from "redux";

export interface SetErrorMessageAction extends Action {
    message: string;
}

export interface AddClientLogAction extends Action {
    logItem: ClientLog;
}

export interface AddServerLogAction extends Action {
    logItem: ServerLog;
}

export function setErrorMessage(message: string): SetErrorMessageAction {
    return { type: "SET_ERROR_MSG", message: message };
}

export function removeErrorMessage(): Action {
    return { type: "REMOVE_ERROR_MSG" };
}

export function addClientLog(logItem: ClientLog): AddClientLogAction {
    return { type: "ADD_CLIENT_LOG", logItem: logItem };
}

export function addServerLog(logItem: ServerLog): AddServerLogAction {
    return { type: "ADD_SERVER_LOG", logItem: logItem };
}

export function addErrorMessage(message: string) {
    return function (dispatch) {
        dispatch(setErrorMessage(message));
        setTimeout(function () { dispatch(removeErrorMessage()); }, 5000);
    }
}

export function addClientError(data: ClientLog) {
    return function (dispatch) {
        dispatch(addErrorMessage(data.message));
        dispatch(addClientLog(data));
    }
}


