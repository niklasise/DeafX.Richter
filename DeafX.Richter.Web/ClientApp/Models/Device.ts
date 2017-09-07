export class Device {

    id: string;
    title: string;
    toggled: boolean;

}

export class ToggleDevice extends Device {

    toggled: boolean;

}

export class ValueDevice extends Device {

    value: string;

}