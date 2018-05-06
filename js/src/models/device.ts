export class Device {

    id: string;
    title: string;

    deviceType: string;
    value: string;
    valueType: string;
    isUpdating: boolean;
    lastChanged: number;
}

export class ToggleDevice extends Device {

    toggled: boolean;
    timer: {
        timerValue: number,
        stateToSet: boolean
    };
    automated: boolean;
    powerConsumption: string;
}

export interface UpdatedDevices {
    devices: Device[],
    lastUpdated: number
}