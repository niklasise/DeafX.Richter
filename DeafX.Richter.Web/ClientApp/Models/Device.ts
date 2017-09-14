export class Device {

    id: string;
    title: string;

    deviceType: string;
}

export class ToggleDevice extends Device {

    toggled: boolean;
}

export class ValueDevice extends Device {

    value: string;
    valueType: string;

}