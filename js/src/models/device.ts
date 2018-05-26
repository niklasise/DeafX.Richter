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

export class WeatherAirDevice extends Device {
    relativeHumidity: number
}

export class WeatherPercipitationDevice extends Device {
    amountTextual: string;
    type: string;
}

export class WeatherWindDevice extends Device {
    maxValue: number;
    direction: number;
    directionTextual: string;
}

export class SunDevice extends Device {
    sunRise: number;
    sunSet: number;
}

export interface UpdatedDevices {
    devices: Device[],
    lastUpdated: number
}
