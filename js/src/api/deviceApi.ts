import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, UpdatedDevices } from "models/device"
import IDeviceListener from "./interfaces/IDeviceListener"
import IDeviceApi from "./interfaces/IDeviceApi"
import ConfigurationUtility from "utilities/configurationUtility";

const TIMEOUT: number = 1000; 

class DeviceApi implements IDeviceApi {

    private _onAllDevicesListener: IDeviceListener;
    private _onDevicesUpdatedListener: IDeviceListener;
    private _lastUpdated: number;
    private _timeoutId: number;
    private _apiBaseUrl;

    constructor() {
        this._apiBaseUrl = ConfigurationUtility.getConfiguration().apiUrl;

        this.fetchUpdatedDevices = this.fetchUpdatedDevices.bind(this);
    }

    public toggleDevice(device: ToggleDeviceModel, toggled: boolean) {
        return this.performRequest(
            this._apiBaseUrl + '/devices/toggle/' + device.id + '/' + toggled,
            'PUT'
        ); 
    }

    public getDevice(deviceId: string) : Promise<DeviceModel> {
        return fetch(`${this._apiBaseUrl}/devices/${deviceId}`, {
            credentials: 'same-origin',
            method: "GET"
        }).then(response => new Promise<DeviceModel>((resolve, reject) => {
            if (response.ok) {
                response.json().then(data => { resolve(data); }).catch(error => { reject(error) });
            }
            else {
                reject("server request responded with status code" + response.status);
            }
        }));
    }

    public setDeviceAutomated(device: ToggleDeviceModel, automated: boolean) {
        return this.performRequest(
            this._apiBaseUrl + '/devices/setAutomated/' + device.id + '/' + automated,
            'PUT'
        );
    }

    public setDeviceTimer(device: ToggleDeviceModel, time: number, state: boolean) {
        return this.performRequest(
            this._apiBaseUrl + '/devices/setTimer/' + device.id + '/' + time + '/' + state,
            'PUT'
        );
    }

    public abortDeviceTimer(device: ToggleDeviceModel) {
        return this.performRequest(
            this._apiBaseUrl + '/devices/abortTimer/' + device.id,
            'PUT'
        );
    }

    public connect(onAllDevices: IDeviceListener, onDevicesUpdated: IDeviceListener) {
        this._onAllDevicesListener = onAllDevices;
        this._onDevicesUpdatedListener = onDevicesUpdated;

        this.performRequest(
            this._apiBaseUrl + '/devices',
            'GET',
            data => {
                this.onAllDevices(data as UpdatedDevices);
                this._timeoutId = window.setTimeout(this.fetchUpdatedDevices, TIMEOUT);
            }
        );        
    }

    public disconnect() {
        clearTimeout(this._timeoutId);
    }

    private fetchUpdatedDevices() {
        this.performRequest(
            this._apiBaseUrl + '/devices/?since=' + this._lastUpdated,
            'GET',
            data => this.onDevicesUpdated(data as UpdatedDevices),
            () => {
                this._timeoutId = window.setTimeout(this.fetchUpdatedDevices, TIMEOUT)
            }
        );
    }

    private performRequest(
        url: string,
        method: string = 'GET',
        onPostJson: (data: any) => void = null,
        onPostRequest: () => void = null)
        : Promise<any>
    {
        return fetch(url, {
            credentials: 'same-origin',
            method: method
        }).then(response => {
            if (response.ok) {
                if (!!onPostJson) {
                    response.json().then(data => { onPostJson(data); }).catch(error => { throw error });
                }
            }
            else {
                throw "Server request responded with status code" + response.status;
            }

            if (!!onPostRequest)
            {
                onPostRequest();
            }
        }).catch(error => {
            throw error;
        });
    }

    private onAllDevices(data: UpdatedDevices) {
        this._lastUpdated = data.lastUpdated;
        this._onAllDevicesListener(data.devices);
    }

    private onDevicesUpdated(data: UpdatedDevices) {
        this._lastUpdated = data.lastUpdated;
        this._onDevicesUpdatedListener(data.devices);
    }

}

export default DeviceApi;