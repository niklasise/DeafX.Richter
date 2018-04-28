import * as SignalR from "@aspnet/signalr-client"
import { Device as DeviceModel, ToggleDevice, UpdatedDevices } from "../Models/Device"
import { IDeviceListener as DeviceListener } from "./IDeviceListener"

class DeviceWSApi {

    private static _connection: SignalR.HubConnection;
    private static _onAllDevicesListener: DeviceListener;
    private static _onDevicesUpdatedListener: DeviceListener;

    static toggleDevice(device: ToggleDevice, toggled: boolean) {
        return DeviceWSApi._connection.invoke("toggleDevice", device.id, toggled);
    }

    static setDeviceAutomated(device: ToggleDevice, automated: boolean) {
        return DeviceWSApi._connection.invoke("setAutomated", device.id, automated);
    }

    static setDeviceTimer(device: ToggleDevice, time: number) {
        return DeviceWSApi._connection.invoke("setTimer", device.id, time, true);
    }

    static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
        DeviceWSApi._onAllDevicesListener = onAllDevices;
        DeviceWSApi._onDevicesUpdatedListener = onDevicesUpdated;

        DeviceWSApi._connection = new SignalR.HubConnection("/devices")
          
        DeviceWSApi._connection.on("allDevices", DeviceWSApi.onAllDevices);
        DeviceWSApi._connection.on("devicesUpdated", DeviceWSApi.onDevicesUpdated);

        DeviceWSApi._connection.start();
    }

    static disconnect() {
        DeviceWSApi._connection.stop();
    }

    private static onAllDevices(data: { devices: DeviceModel[] }) {
        DeviceWSApi._onAllDevicesListener(data.devices);
    }

    private static onDevicesUpdated(data: { devices: DeviceModel[] }) {
        DeviceWSApi._onDevicesUpdatedListener(data.devices);
    }

}

export default DeviceWSApi;