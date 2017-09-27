export class Device {

    id: string;
    title: string;

    deviceType: string;
    lastUpdated: number;
    isUpdating: boolean;
}

export class ToggleDevice extends Device {

    toggled: boolean;
    setTimerActive: boolean;
    timerValue: number;

}

export class ValueDevice extends Device {

    value: string;
    valueType: string;

}

export interface UpdatedDevices {
    devices: Device[],
    timestamp: number
}