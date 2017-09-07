import { Reducer, Action } from "redux";
import { Device as DeviceModel } from "../Models/Device";
import { ToggleDeviceAction } from "../Actions/DeviceActions";

export interface DeviceState {
    deviceList: DeviceModel[];
}

const deviceReducer: Reducer<DeviceState> = (state = { deviceList: [] }, action: Action) =>
{
    switch (action.type) {
        case "TOGGLE_DEVICE":
            return toggleDevice((action as ToggleDeviceAction).device, state);
        default:
            return state;
    }
}

function toggleDevice(device: DeviceModel, state: DeviceState) : DeviceState
{
    const deviceIndex = state.deviceList.findIndex(i => i.id === device.id);

    // Device can't be found, return untouched state
    if (deviceIndex > 0)
    {
        return state;
    }

    const devices: DeviceModel[] = [
            ...state.deviceList.slice(0, deviceIndex),
            {...device, toggled: !device.toggled },
            ...state.deviceList.slice(0, deviceIndex + 1)
        ]

    return { ...state, deviceList: devices };
}

export default deviceReducer;