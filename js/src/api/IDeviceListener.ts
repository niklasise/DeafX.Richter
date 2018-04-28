import { Device as DeviceModel } from "models/Device"

export interface IDeviceListener {
    (devices: DeviceModel[]): any
}