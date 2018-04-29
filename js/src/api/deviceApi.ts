import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel, UpdatedDevices } from "models/device"
import { IDeviceListener as DeviceListener } from "./IDeviceListener"

const TIMEOUT: number = 1000; 

class DeviceApi {

    private static _onAllDevicesListener: DeviceListener;
    private static _onDevicesUpdatedListener: DeviceListener;
    private static _lastUpdated: number;
    private static _timeoutId: number;
     

    static toggleDevice(device: ToggleDeviceModel, toggled: boolean) {
        return DeviceApi.performRequest(
            '/api/devices/toggle/' + device.id + '/' + toggled,
            'PUT'
        ); 
    }

    static getDevice(deviceId: string) : Promise<DeviceModel> {
        //return DeviceApi.performRequest(
        //    `/api/devices/${deviceId}`,
        //    'PUT'
        //);

        return fetch(`/api/devices/${deviceId}`, {
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

    static setDeviceAutomated(device: ToggleDeviceModel, automated: boolean) {
        return DeviceApi.performRequest(
            '/api/devices/setAutomated/' + device.id + '/' + automated,
            'PUT'
        );
    }

    static setDeviceTimer(device: ToggleDeviceModel, time: number, state: boolean) {
        return DeviceApi.performRequest(
            '/api/devices/setTimer/' + device.id + '/' + time + '/' + state,
            'PUT'
        );
    }

    static abortDeviceTimer(device: ToggleDeviceModel) {
        return DeviceApi.performRequest(
            '/api/devices/abortTimer/' + device.id,
            'PUT'
        );
    }

    static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
        DeviceApi._onAllDevicesListener = onAllDevices;
        DeviceApi._onDevicesUpdatedListener = onDevicesUpdated;

        DeviceApi.performRequest(
            '/api/devices',
            'GET',
            data => {
                DeviceApi.onAllDevices(data as UpdatedDevices);
                DeviceApi._timeoutId = window.setTimeout(DeviceApi.fetchUpdatedDevices, TIMEOUT);
            }
        );        
    }

    static disconnect() {
        clearTimeout(DeviceApi._timeoutId);
    }

    private static fetchUpdatedDevices() {
        DeviceApi.performRequest(
            '/api/devices/?since=' + DeviceApi._lastUpdated,
            'GET',
            data => DeviceApi.onDevicesUpdated(data as UpdatedDevices),
            () => {
                DeviceApi._timeoutId = window.setTimeout(DeviceApi.fetchUpdatedDevices, TIMEOUT)
            }
        );
    }

    private static performRequest(
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

    private static onAllDevices(data: UpdatedDevices) {
        DeviceApi._lastUpdated = data.lastUpdated;
        DeviceApi._onAllDevicesListener(data.devices);
    }

    private static onDevicesUpdated(data: UpdatedDevices) {
        DeviceApi._lastUpdated = data.lastUpdated;
        DeviceApi._onDevicesUpdatedListener(data.devices);
    }

}

export default DeviceApi;