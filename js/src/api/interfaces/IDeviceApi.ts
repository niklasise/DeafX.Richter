import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel } from "models/device";
import IDeviceListener from "./IDeviceListener"

export default interface IDeviceApi {

    getDevice(deviceId: string): Promise<DeviceModel>;

    toggleDevice(device: ToggleDeviceModel, toggled: boolean);

    setDeviceAutomated(device: ToggleDeviceModel, automated: boolean);

    setDeviceTimer(device: ToggleDeviceModel, time: number, state: boolean);

    abortDeviceTimer(device: ToggleDeviceModel);

    connect(onAllDevices: IDeviceListener, onDevicesUpdated: IDeviceListener);

    disconnect();

}