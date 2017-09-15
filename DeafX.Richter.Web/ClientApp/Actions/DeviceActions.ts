import { Device as DeviceModel, ToggleDevice } from "../Models/Device";
import { Action } from "redux";
import deviceApi from "../Api/mockDeviceApi";

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

export function devicesUpdated(devices: DeviceModel[]): DevicesUpdatedAction {
    return { type: "DEVICES_UPDATED", devices: devices };
}

export function loadDevicesSuccess(devices: DeviceModel[]): LoadDevicesSuccessAction {
    return { type: "LOAD_DEVICES_SUCCESS", devices: devices };
}

export function toggleDevice(device: ToggleDevice) {
    return function (dispatch) {
        return Promise.all([
            dispatch(toggleDeviceStarted(device)),
            deviceApi.toggleDevice(device).catch(error => { throw error })
        ]);
    }
}

export function loadDevicesAndListenForUpdates() {
    return function (dispatch) {
        return deviceApi.getAlldevices().then(devices => {
            dispatch(loadDevicesSuccess(devices as DeviceModel[]));
            deviceApi.listenForDeviceUpdates(event => dispatch(devicesUpdated(event.data)));
        }).catch(error => {
            throw error;
        });
    }
}

export function stopListeningForDeviceUpdates() {
    return function (dispatch) {
        deviceApi.stopListeningForDeviceUpdates();
    }
}
