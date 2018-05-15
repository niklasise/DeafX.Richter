import { Device as DeviceModel } from "models/Device"

export default interface IDeviceListener {
    (devices: DeviceModel[]): any
}