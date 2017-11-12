import { Device as DeviceModel } from "../Models/Device"

export interface IDeviceListener {
    (devices: DeviceModel[]): any
}