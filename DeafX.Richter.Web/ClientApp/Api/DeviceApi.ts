//import * as SignalR from "@aspnet/signalr-client"
import { Device as DeviceModel, ToggleDevice, UpdatedDevices } from "../Models/Device"
import { IDeviceListener as DeviceListener } from "./IDeviceListener"

const TIMEOUT: number = 1000; 

class DeviceApi {

    //private static _connection: SignalR.HubConnection;
    private static _onAllDevicesListener: DeviceListener;
    private static _onDevicesUpdatedListener: DeviceListener;
    private static _lastUpdated: number;
    private static _timeoutId: number;
     

    static toggleDevice(device: ToggleDevice, toggled: boolean) {
        return DeviceApi.performRequest(
            'api/devices/toggle/' + device.id + '/' + toggled,
            'PUT'
        ); 
    }

    static setDeviceAutomated(device: ToggleDevice, automated: boolean) {
        return DeviceApi.performRequest(
            'api/devices/setAutomated/' + device.id + '/' + automated,
            'PUT'
        );
    }

    static setDeviceTimer(device: ToggleDevice, time: number) {
        return DeviceApi.performRequest(
            'api/devices/setTimer/' + device.id + '/' + time + '/' + true,
            'PUT'
        );
    }

    static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
        DeviceApi._onAllDevicesListener = onAllDevices;
        DeviceApi._onDevicesUpdatedListener = onDevicesUpdated;

        DeviceApi.performRequest(
            'api/devices',
            'GET',
            data => {
                DeviceApi.onAllDevices(data as UpdatedDevices);
                DeviceApi._timeoutId = setTimeout(DeviceApi.fetchUpdatedDevices, TIMEOUT);
            }
        );        
    }

    static disconnect() {
        clearTimeout(DeviceApi._timeoutId);
    }

    private static fetchUpdatedDevices() {
        DeviceApi.performRequest(
            'api/devices/' + DeviceApi._lastUpdated,
            'GET',
            data => DeviceApi.onDevicesUpdated(data as UpdatedDevices),
            () => {
                DeviceApi._timeoutId = setTimeout(DeviceApi.fetchUpdatedDevices, TIMEOUT)
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