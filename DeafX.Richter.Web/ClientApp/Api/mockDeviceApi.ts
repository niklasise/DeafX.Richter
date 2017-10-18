import delay from './delay';
import { Device as DeviceModel, ToggleDevice, ValueDevice, UpdatedDevices } from "../Models/Device"

// This file mocks a web API by working with the hard-coded data below.
// It uses setTimeout to simulate the delay of an AJAX call.
// All calls return promises.
let devices : DeviceModel[] = [
    {
        id: "1",
        title: "Vardagsrum",
        toggled: true,
        deviceType: "TOGGLE_DEVICE",
        timerValue: 3672
    } as ToggleDevice,
    {
        id: "2",
        title: "Gästrum",
        toggled: false,
        deviceType: "TOGGLE_DEVICE",
    } as ToggleDevice,
    {
        id: "3",
        title: "Hall Nedervåning",
        value: "21",
        deviceType: "VALUE_DEVICE",
    } as ValueDevice,
    {
        id: "4",
        title: "Hall övervåning",
        value: "23",
        deviceType: "VALUE_DEVICE",
    } as ValueDevice,
];

let deviceListeners: DeviceListener[] = [];
let lastUpdateSent: number = 0;

interface DeviceListener {
    (event: { data: DeviceModel[] }) : any
}

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

//This would be performed on the server in a real app. Just stubbing in.
const generateId = (device) => {
    return replaceAll(device.title, ' ', '-');
};

class deviceApi {
    static getAlldevices(): Promise<DeviceModel[]> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(Object.assign([], devices));
            }, delay);
        });
    }

    static toggleDevice(device: ToggleDevice) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDevice);

                toggleDevice.toggled = !toggleDevice.toggled;
                toggleDevice.lastUpdated = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static setDeviceTimer(device: ToggleDevice, time: number) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDevice);

                toggleDevice.timerValue = time;
                toggleDevice.lastUpdated = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static getUpdatedDevices(since: number): Promise<UpdatedDevices> {
        console.log("Updating devices");
        return new Promise<UpdatedDevices>((resolve, reject) => {
            setTimeout(() => {
                resolve({ devices: devices.filter(device => device.lastUpdated > since), timestamp: new Date().getTime() });
            }, delay);
        });
    }

    static listenForDeviceUpdates(listener: DeviceListener) {
        deviceListeners.push(listener);
    }

    static stopListeningForDeviceUpdates() {
        deviceListeners = [];
    }

    private static alertDeviceListeners() {
        deviceListeners.forEach(val => {
            val({ data: devices.filter(device => device.lastUpdated > lastUpdateSent) });
        });

        lastUpdateSent = new Date().getTime();
    }

    static startRandomizingValueDevices() {
        deviceApi.randomizeValueDevices();
    }

    private static randomizeValueDevices()
    {
        setTimeout(function () {
            devices.forEach((val, index, arr) => {
                if (val.deviceType !== "VALUE_DEVICE") {
                    return;
                }

                (val as ValueDevice).value = "" + Math.round(20 + Math.random() * 5);
                val.lastUpdated = new Date().getTime();
            });

            deviceApi.alertDeviceListeners();

            deviceApi.randomizeValueDevices();
        }, 5000);
    }

}

export default deviceApi;