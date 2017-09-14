import { Reducer, Action } from "redux";
import { Device as DeviceModel, ToggleDevice } from "../Models/Device";
import { ToggleDeviceAction, LoadDevicesSuccessAction } from "../Actions/DeviceActions";

export interface DeviceState {
    deviceList: DeviceModel[];
}

const deviceReducer: Reducer<DeviceState> = (state = { deviceList: [] }, action: Action) =>
{
    switch (action.type) {
        case "TOGGLE_DEVICE":
            return toggleDevice((action as ToggleDeviceAction).device as ToggleDevice, state);
        case "LOAD_DEVICES_SUCCESS":
            return { ...state, deviceList: (action as LoadDevicesSuccessAction).devices };
        default:
            return state;
    }
}


function toggleDevice(device: ToggleDevice, state: DeviceState) : DeviceState
{
    debugger;
    const deviceIndex = state.deviceList.findIndex(i => i.id === device.id);

    // Device can't be found, return untouched state
    if (deviceIndex < 0)
    {
        return state;
    }

    const devices: DeviceModel[] = [
            ...state.deviceList.slice(0, deviceIndex),
            { ...device, toggled: !device.toggled } as ToggleDevice,
            ...state.deviceList.slice(deviceIndex + 1)
        ]

    return { ...state, deviceList: devices };
}

export default deviceReducer;