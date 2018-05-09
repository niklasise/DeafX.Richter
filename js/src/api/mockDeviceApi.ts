import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, UpdatedDevices, WeatherAirDevice, WeatherPercipitationDevice, WeatherWindDevice } from "models/Device"
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
    {
        id: "5",
        title: "Lufttemperatur",
        value: "2.3",
        deviceType: "WEATHER_AIR_DEVICE",
        relativeHumidity: 48.5
    } as WeatherAirDevice,
    {
        id: "6",
        title: "Nederbörd",
        value: "2.2",
        deviceType: "WEATHER_PERCIP_DEVICE",
        type: "Regn"
    } as WeatherPercipitationDevice,
    {
        id: "7",
        title: "Vind",
        value: "3.4",
        deviceType: "WEATHER_WIND_DEVICE",
        direction: 180,
        maxValue: 4.8
    } as WeatherWindDevice,
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
                if (val.deviceType === "TOGGLE_DEVICE") {
                    return;
                }

                if(val.deviceType === "WEATHER_AIR_DEVICE") {
                    deviceApi.randomizeAirDevice(val as WeatherAirDevice);
                }
                else if(val.deviceType === "WEATHER_WIND_DEVICE"){
                    deviceApi.randomizeWindDevice(val as WeatherWindDevice);
                }
                else{
                    deviceApi.randomizeValueDevice(val);
                }
                
                val.lastChanged = new Date().getTime();
            });

            deviceApi.alertDeviceListeners();

            deviceApi.randomizeValueDevices();
        }, 5000);
    }

    static randomizeValueDevice(device: DeviceModel){
        device.value = "" + Math.round(20 + Math.random() * 5)
    }

    static randomizeAirDevice(device: WeatherAirDevice){
        device.value = "" + Math.round(20 + Math.random() * 5);
        device.relativeHumidity = Math.round(50 + Math.random() * 5);
    }

    static randomizeWindDevice(device: WeatherWindDevice){
        device.value = "" + Math.round(2 + Math.random() * 5);
        device.maxValue = Math.round(3 + Math.random() * 5);
        device.direction = Math.round(180 + Math.random() * 5);
    }

}

deviceApi.randomizeValueDevices();

export default deviceApi;