import { Device as DeviceModel, ToggleDevice } from "models/device";
import { Action } from "redux";
//import deviceApi from "../Api/MockDeviceApi";
import deviceApi from "api/deviceApi";

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

export function abortTimerDevice(device: ToggleDevice) {
    return function (dispatch) {
        return Promise.all([
            dispatch(abortTimerDeviceStarted(device)),
            deviceApi.abortDeviceTimer(device).catch(error => { throw error })
        ]);
    }
}

export function setTimerDevice(device: ToggleDevice, time: number, state: boolean) {
    return function (dispatch) {
        return Promise.all([
            dispatch(setTimerDeviceStarted(device)),
            deviceApi.setDeviceTimer(device, time, state).catch(error => { throw error })
        ]);
    }
}

export function toggleDevice(device: ToggleDevice, toggled: boolean) {
    return function (dispatch) {
        return Promise.all([
            dispatch(toggleDeviceStarted(device)),
            deviceApi.toggleDevice(device, toggled).catch(error => { throw error })
        ]);
    }
}

export function setDeviceAutomated(device: ToggleDevice, automated: boolean) {
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
