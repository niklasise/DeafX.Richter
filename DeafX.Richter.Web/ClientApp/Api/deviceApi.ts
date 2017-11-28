import * as SignalR from "@aspnet/signalr-client"
import { Device as DeviceModel, ToggleDevice, UpdatedDevices } from "../Models/Device"
import { IDeviceListener as DeviceListener } from "./IDeviceListener"

class DeviceApi {

    private static _connection: SignalR.HubConnection;
    private static _onAllDevicesListener: DeviceListener;
    private static _onDevicesUpdatedListener: DeviceListener;

    static toggleDevice(device: ToggleDevice, toggled: boolean) {
        return DeviceApi._connection.invoke("toggleDevice", device.id, toggled);
    }

    static setDeviceAutomated(device: ToggleDevice, automated: boolean) {
        return DeviceApi._connection.invoke("setAutomated", device.id, automated);
    }

    static setDeviceTimer(device: ToggleDevice, time: number) {
        return new Promise((resolve, reject) => {

        });
    }

    static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
        DeviceApi._onAllDevicesListener = onAllDevices;
        DeviceApi._onDevicesUpdatedListener = onDevicesUpdated;

        DeviceApi._connection = new SignalR.HubConnection("/devices")
          
        DeviceApi._connection.on("allDevices", DeviceApi.onAllDevices);
        DeviceApi._connection.on("devicesUpdated", DeviceApi.onDevicesUpdated);

        DeviceApi._connection.start();
    }

    static disconnect() {
        DeviceApi._connection.stop();
    }

    private static onAllDevices(data: { devices: DeviceModel[] }) {
        DeviceApi._onAllDevicesListener(data.devices);
    }

    private static onDevicesUpdated(data: { devices: DeviceModel[] }) {
        DeviceApi._onDevicesUpdatedListener(data.devices);
    }

}

export default DeviceApi;