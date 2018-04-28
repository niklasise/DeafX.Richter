import { Reducer, Action } from "redux";
import { Device as DeviceModel, ToggleDevice } from "models/device";
import { ToggleDeviceAction, LoadDevicesSuccessAction, DevicesUpdatedAction } from "actions/deviceActions";

export interface DeviceState {
    deviceList: DeviceModel[];
}

const deviceReducer: Reducer<DeviceState> = (state = { deviceList: [] }, action: Action) =>
{
    switch (action.type) {
        case "SET_TIMER_DEVICE_STARTED":
        case "SET_DEVICE_AUTOMATED_STARTED":
        case "TOGGLE_DEVICE_STARTED":
        case "ABORT_TIMER_DEVICE_STARTED":
            return setDeviceIsUpdating((action as ToggleDeviceAction).device as ToggleDevice, state);
        case "LOAD_DEVICES_SUCCESS":
            return updateDevices((action as LoadDevicesSuccessAction).devices, state);
        case "DEVICES_UPDATED":
            return updateDevices((action as DevicesUpdatedAction).devices, state);
        default:
            return state;
    }
}

function updateDevices(updatedDevices: DeviceModel[], state: DeviceState) : DeviceState
{
    console.log("Updated devices fetched - " + updatedDevices.length);
    if (!updatedDevices.length)
    {
        return state;
    }

    let newDeviceList: DeviceModel[] = [ ...state.deviceList ];
    
    updatedDevices.forEach((device) => {
        let index = state.deviceList.findIndex(val => val.id === device.id);

        if (index >= 0)
        {
            newDeviceList[index] = Object.assign({}, device, { isUpdating: false });
        }
        else {
            newDeviceList.push(device);
        }
    });

    return {
        ...state, deviceList: newDeviceList
    };
}

function setDeviceIsUpdating(device: DeviceModel, state: DeviceState) : DeviceState
{
    const deviceIndex = state.deviceList.findIndex(i => i.id === device.id);

    // Device can't be found, return untouched state
    if (deviceIndex < 0)
    {
        return state;
    }

    const devices: DeviceModel[] = [
            ...state.deviceList.slice(0, deviceIndex),
            { ...device, isUpdating: true },
            ...state.deviceList.slice(deviceIndex + 1)
        ]

    return { ...state, deviceList: devices };
}

export default deviceReducer;