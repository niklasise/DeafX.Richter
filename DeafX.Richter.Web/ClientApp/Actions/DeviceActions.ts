import { Device as DeviceModel } from "../Models/Device";
import { Action } from "redux";

export interface ToggleDeviceAction extends Action {
    device: DeviceModel;
}

export function toggleDevice(device: DeviceModel): ToggleDeviceAction {
    return { type: "TOGGLE_DEVICE", device: device };
}