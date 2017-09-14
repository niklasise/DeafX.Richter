import delay from './delay';
import { Device as DeviceModel, ToggleDevice, ValueDevice } from "../Models/Device"

// This file mocks a web API by working with the hard-coded data below.
// It uses setTimeout to simulate the delay of an AJAX call.
// All calls return promises.
const devices : DeviceModel[] = [
    {
        id: "1",
        title: "Vardagsrum",
        toggled: true,
        deviceType: "TOGGLE_DEVICE"
    } as ToggleDevice,
    {
        id: "2",
        title: "Gästrum",
        toggled: false,
        deviceType: "TOGGLE_DEVICE"
    } as ToggleDevice,
    {
        id: "3",
        title: "Hall Nedervåning",
        value: "21",
        deviceType: "VALUE_DEVICE"
    } as ValueDevice,
    {
        id: "4",
        title: "Hall övervåning",
        value: "23",
        deviceType: "VALUE_DEVICE"
    } as ValueDevice,
];

function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}

//This would be performed on the server in a real app. Just stubbing in.
const generateId = (device) => {
    return replaceAll(device.title, ' ', '-');
};

class deviceApi {
    static getAlldevices() {
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                resolve(Object.assign([], devices));
            }, delay);
        });
    }

    static savedevice(device) {
        device = Object.assign({}, device); // to avoid manipulating object passed in.
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                // Simulate server-side validation
                const mindeviceTitleLength = 1;
                if (device.title.length < mindeviceTitleLength) {
                    reject(`Title must be at least ${mindeviceTitleLength} characters.`);
                }

                if (device.id) {
                    const existingdeviceIndex = devices.findIndex(a => a.id == device.id);
                    devices.splice(existingdeviceIndex, 1, device);
                } else {
                    //Just simulating creation here.
                    //The server would generate ids and watchHref's for new devices in a real app.
                    //Cloning so copy returned is passed by value rather than by reference.
                    device.id = generateId(device);
                    device.watchHref = `http://www.pluralsight.com/devices/${device.id}`;
                    devices.push(device);
                }

                resolve(device);
            }, delay);
        });
    }

    //static deletedevice(deviceId) {
    //    return new Promise((resolve, reject) => {
    //        setTimeout(() => {
    //            const indexOfdeviceToDelete = devices.findIndex(device => {
    //                device.id == deviceId;
    //            });
    //            devices.splice(indexOfdeviceToDelete, 1);
    //            resolve();
    //        }, delay);
    //    });
    //}
}

export default deviceApi;