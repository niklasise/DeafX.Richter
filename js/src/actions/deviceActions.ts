import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel } from "models/device";
import { Action } from "redux";
import DeviceApi from "@api/deviceApi";

const deviceApi = new DeviceApi();

export interface ToggleDeviceAction extends Action {
    device: DeviceModel;
}

export interface LoadDevicesSuccessAction extends Action {
    devices: DeviceModel[];
}

export interface DevicesUpdatedAction extends Action {
    devices: DeviceModel[];
}

export function toggleDeviceStarted(device: DeviceModel): ToggleDeviceAction {
    return { type: "TOGGLE_DEVICE_STARTED", device: device };
}

export function setDeviceAutomatedStarted(device: DeviceModel): ToggleDeviceAction {
    return { type: "SET_DEVICE_AUTOMATED_STARTED", device: device };
}

export function setTimerDeviceStarted(device: DeviceModel): ToggleDeviceAction {
    return { type: "SET_TIMER_DEVICE_STARTED", device: device };
}

export function abortTimerDeviceStarted(device: DeviceModel): ToggleDeviceAction {
    return { type: "ABORT_TIMER_DEVICE_STARTED", device: device };
}

export function devicesUpdated(devices: DeviceModel[]): DevicesUpdatedAction {
    return { type: "DEVICES_UPDATED", devices: devices };
}

export function loadDevicesSuccess(devices: DeviceModel[]): LoadDevicesSuccessAction {
    return { type: "LOAD_DEVICES_SUCCESS", devices: devices };
}

export function abortTimerDevice(device: ToggleDeviceModel) {
    return function (dispatch) {
        return Promise.all([
            dispatch(abortTimerDeviceStarted(device)),
            deviceApi.abortDeviceTimer(device).catch(error => { throw error })
        ]);
    }
}

export function setTimerDevice(device: ToggleDeviceModel, time: number, state: boolean) {
    return function (dispatch) {
        return Promise.all([
            dispatch(setTimerDeviceStarted(device)),
            deviceApi.setDeviceTimer(device, time, state).catch(error => { throw error })
        ]);
    }
}

export function toggleDevice(device: ToggleDeviceModel, toggled: boolean) {
    return function (dispatch) {
        return Promise.all([
            dispatch(toggleDeviceStarted(device)),
            deviceApi.toggleDevice(device, toggled).catch(error => { throw error })
        ]);
    }
}

export function setDeviceAutomated(device: ToggleDeviceModel, automated: boolean) {
    return function (dispatch) {
        return Promise.all([
            dispatch(setDeviceAutomatedStarted(device)),
            deviceApi.setDeviceAutomated(device, automated).catch(error => { throw error })
        ]);
    }
}

export function connectToDeviceApi() {
    return function (dispatch) {
        deviceApi.connect(
            devices => dispatch(loadDevicesSuccess(devices)),
            devices => dispatch(devicesUpdated(devices))
        );
    }
}

export function disconnectFromDeviceApi() {
    return function (dispatch) {
        deviceApi.disconnect();
    }
}
