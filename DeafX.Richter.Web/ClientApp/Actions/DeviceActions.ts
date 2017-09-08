import { Device as DeviceModel } from "../Models/Device";
import { Action } from "redux";
import deviceApi from "../Api/mockDeviceApi";

export interface ToggleDeviceAction extends Action {
    device: DeviceModel;
}

export interface LoadDevicesSuccessAction extends Action {
    devices: DeviceModel[];
}

export function toggleDevice(device: DeviceModel): ToggleDeviceAction {
    return { type: "TOGGLE_DEVICE", device: device };
}

export function loadDevicesSuccess(devices: DeviceModel[]): LoadDevicesSuccessAction {
    return { type: "LOAD_DEVICES_SUCCESS", devices: devices };
}

export function loadDevices() {
    return function (dispatch) {
        return deviceApi.getAlldevices().then(devices => {
            dispatch(loadDevicesSuccess(devices as DeviceModel[]));
        }).catch(error => {
            throw error;
        });
    }
}