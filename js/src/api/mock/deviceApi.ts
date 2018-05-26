import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, UpdatedDevices, WeatherAirDevice, WeatherPercipitationDevice, WeatherWindDevice, SunDevice } from "models/Device"
import IDeviceListener from "../interfaces/IDeviceListener"
import IDeviceApi from "../interfaces/IDeviceApi";

const DELAY : number = 1000;

class deviceApi implements IDeviceApi {

    private devices : DeviceModel[] = [
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
        {
            id: "8",
            title: "Sol",
            value: "12,3",
            deviceType: "SUN_DEVICE",
            sunRise: 1527305437,
            sunSet: 1527371270
        } as SunDevice,
    ];  

    private deviceListeners: IDeviceListener[] = [];
    private lastUpdateSent: number = 0;

    constructor() {
        this.randomizeValueDevices();       
    }

    public getAlldevices(): Promise<DeviceModel[]> {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(Object.assign([], this.devices));
            }, DELAY);
        });
    }

    public getDevice(deviceId: string): Promise<DeviceModel> {
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
            }, DELAY);
        });
    }

    public toggleDevice(device: ToggleDeviceModel, toggled: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = this.devices.findIndex(i => i.id === device.id);

                let toggleDevice = (this.devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.toggled = toggled;
                toggleDevice.lastChanged = new Date().getTime();

                this.alertDeviceListeners();

                resolve();
            }, DELAY);
        });
    }

    public setDeviceAutomated(device: ToggleDeviceModel, automated: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = this.devices.findIndex(i => i.id === device.id);

                let toggleDevice = (this.devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.automated = !toggleDevice.automated;
                toggleDevice.lastChanged = new Date().getTime();

                this.alertDeviceListeners();

                resolve();
            }, DELAY);
        });
    }

    public setDeviceTimer(device: ToggleDeviceModel, time: number, state: boolean) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = this.devices.findIndex(i => i.id === device.id);

                let toggleDevice = (this.devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.timer = {
                    timerValue: time,
                    stateToSet: state
                };
                toggleDevice.lastChanged = new Date().getTime();

                this.alertDeviceListeners();

                resolve();
            }, DELAY);
        });
    }

    public abortDeviceTimer(device: ToggleDeviceModel) {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                const deviceIndex = this.devices.findIndex(i => i.id === device.id);

                let toggleDevice = (this.devices[deviceIndex] as ToggleDeviceModel);

                toggleDevice.timer = null;
                toggleDevice.lastChanged = new Date().getTime();

                this.alertDeviceListeners();

                resolve();
            }, DELAY);
        });
    }

    public connect(onAllDevices: IDeviceListener, onDevicesUpdated: IDeviceListener) {
        this.getAlldevices().then(devices => onAllDevices(devices));
        this.deviceListeners.push(onDevicesUpdated);
    }

    public disconnect() {
        this.deviceListeners = [];
    }

    private alertDeviceListeners() {
        this.deviceListeners.forEach(val => {
            val(this.devices.filter(device => device.lastChanged > this.lastUpdateSent));
        });

        this.lastUpdateSent = new Date().getTime();
    }

    private randomizeValueDevices()
    {
        var self = this;

        setTimeout(function () {
            self.devices.forEach((val, index, arr) => {
                if (val.deviceType === "TOGGLE_DEVICE") {
                    return;
                }

                if(val.deviceType === "WEATHER_AIR_DEVICE") {
                    self.randomizeAirDevice(val as WeatherAirDevice);
                }
                else if(val.deviceType === "WEATHER_WIND_DEVICE"){
                    self.randomizeWindDevice(val as WeatherWindDevice);
                }
                else{
                    self.randomizeValueDevice(val);
                }
                
                val.lastChanged = new Date().getTime();
            });

            self.alertDeviceListeners();

            self.randomizeValueDevices();
        }, 5000);
    }

    private randomizeValueDevice(device: DeviceModel){
        device.value = "" + Math.round(20 + Math.random() * 5)
    }

    private randomizeAirDevice(device: WeatherAirDevice){
        device.value = "" + Math.round(20 + Math.random() * 5);
        device.relativeHumidity = Math.round(50 + Math.random() * 5);
    }

    private randomizeWindDevice(device: WeatherWindDevice){
        device.value = "" + Math.round(2 + Math.random() * 5);
        device.maxValue = Math.round(3 + Math.random() * 5);
        device.direction = Math.round(180 + Math.random() * 5);
    }

}

export default deviceApi;