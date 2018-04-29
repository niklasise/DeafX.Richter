﻿import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, UpdatedDevices } from "models/Device"
import { IDeviceListener as DeviceListener } from "./IDeviceListener"

let delay : number = 1000;

// This file mocks a web API by working with the hard-coded data below.
// It uses setTimeout to simulate the delay of an AJAX call.
// All calls return promises.
let devices : DeviceModel[] = [
    {
        id: "1",
        title: "Vardagsrum",
        toggled: true,
        deviceType: "TOGGLE_DEVICE",
        timer: {
            timerValue: 3672,
            stateToSet: true
        },
        automated: true
    } as ToggleDeviceModel,
    {
        id: "2",
        title: "Gästrum",
        toggled: false,
        deviceType: "TOGGLE_DEVICE",
        automated: false
    } as ToggleDeviceModel,
    {
        id: "3",
        title: "Hall Nedervåning",
        value: "21",
        deviceType: "VALUE_DEVICE",
    } as DeviceModel,
    {
        id: "4",
        title: "Hall övervåning",
        value: "23",
        deviceType: "VALUE_DEVICE",
    } as DeviceModel,
];

let deviceListeners: DeviceListener[] = [];
let lastUpdateSent: number = 0;

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

    static getDevice(deviceId: string): Promise<DeviceModel> {
        return new Promise<DeviceModel>((resolve, reject) => {
            setTimeout(() => {
                if (deviceId === "def456")
                {
                    reject();
                }
                else {
                    resolve({
                        id: deviceId,
                        title: deviceId + " name",
                        value: "21",
                        deviceType: "VALUE_DEVICE"
                    } as DeviceModel);
                }
            }, delay);
        });
    }

    static toggleDevice(device: ToggleDeviceModel, toggled: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.toggled = toggled;
                toggleDevice.lastChanged = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static setDeviceAutomated(device: ToggleDeviceModel, automated: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.automated = !toggleDevice.automated;
                toggleDevice.lastChanged = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static setDeviceTimer(device: ToggleDeviceModel, time: number, state: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.timer = {
                    timerValue: time,
                    stateToSet: state
                };
                toggleDevice.lastChanged = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static abortDeviceTimer(device: ToggleDeviceModel) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = devices.findIndex(i => i.id === device.id);

                let toggleDevice = (devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.timer = null;
                toggleDevice.lastChanged = new Date().getTime();

                deviceApi.alertDeviceListeners();

                resolve();
            }, delay);
        });
    }

    static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
        deviceApi.getAlldevices().then(devices => onAllDevices(devices));
        deviceListeners.push(onDevicesUpdated);
    }

    static disconnect() {
        deviceListeners = [];
    }

    private static alertDeviceListeners() {
        deviceListeners.forEach(val => {
            val(devices.filter(device => device.lastChanged > lastUpdateSent));
        });

        lastUpdateSent = new Date().getTime();
    }

    static randomizeValueDevices()
    {
        setTimeout(function () {
            devices.forEach((val, index, arr) => {
                if (val.deviceType !== "VALUE_DEVICE") {
                    return;
                }

                val.value = "" + Math.round(20 + Math.random() * 5);
                val.lastChanged = new Date().getTime();
            });

            deviceApi.alertDeviceListeners();

            deviceApi.randomizeValueDevices();
        }, 5000);
    }

}

deviceApi.randomizeValueDevices();

export default deviceApi;