export class Device {

    id: string;
    title: string;

    deviceType: string;
    value: string;
    valueType: string;
    isUpdating: boolean;
    lastUpdated: number;
}

export class ToggleDevice extends Device {

    toggled: boolean;
    timerValue: number;
    automated: boolean;

}

export interface UpdatedDevices {
    devices: Device[],
    lastUpdated: number
}