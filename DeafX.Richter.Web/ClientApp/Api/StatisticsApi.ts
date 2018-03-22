class StatisticsApi {

    //private static _onAllDevicesListener: DeviceListener;
    //private static _onDevicesUpdatedListener: DeviceListener;
    //private static _lastUpdated: number;
    //private static _timeoutId: number;

    static getStatistics(deviceId: string): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            fetch('api/statistics/' + deviceId, {
                method: 'GET',
                headers: {
                    'Accept': 'application/json',
                }
            }).then(response => {
                if (response.ok) {
                    resolve();
                }
                else if (response.status === 400)
                {
                    response.json().then(data => reject(data)).catch(error => { reject() });
                }
                else {
                    reject(Error("Server request responded with status code: " + response.status));
                }
            }).catch(error => {
                reject(error);
            });

        });
    }

    //static toggleDevice(device: ToggleDevice, toggled: boolean) {
    //    return DeviceApi.performRequest(
    //        'api/devices/toggle/' + device.id + '/' + toggled,
    //        'PUT'
    //    );
    //}

    //static setDeviceAutomated(device: ToggleDevice, automated: boolean) {
    //    return DeviceApi.performRequest(
    //        'api/devices/setAutomated/' + device.id + '/' + automated,
    //        'PUT'
    //    );
    //}

    //static setDeviceTimer(device: ToggleDevice, time: number) {
    //    return DeviceApi.performRequest(
    //        'api/devices/setTimer/' + device.id + '/' + time + '/' + true,
    //        'PUT'
    //    );
    //}

    //static connect(onAllDevices: DeviceListener, onDevicesUpdated: DeviceListener) {
    //    DeviceApi._onAllDevicesListener = onAllDevices;
    //    DeviceApi._onDevicesUpdatedListener = onDevicesUpdated;

    //    DeviceApi.performRequest(
    //        'api/devices',
    //        'GET',
    //        data => {
    //            DeviceApi.onAllDevices(data as UpdatedDevices);
    //            DeviceApi._timeoutId = setTimeout(DeviceApi.fetchUpdatedDevices, TIMEOUT);
    //        }
    //    );
    //}

}

export default StatisticsApi;