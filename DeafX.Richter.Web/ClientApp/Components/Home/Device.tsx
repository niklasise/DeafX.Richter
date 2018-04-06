import * as React from "react";
import { Link, } from "react-router-dom";
import { Device as DeviceModel } from "../../Models/Device";
import ToggleDevice from "./ToggleDevice";
import ValueDevice from "./ValueDevice";

export interface DeviceProps {
    device: DeviceModel;
    onIconClick(source: DeviceModel): void;
    onConfigClick(source: DeviceModel): void;
    onTimerClick(source: DeviceModel): void;
    onTimerAbortClick(source: DeviceModel): void;
    onAutomatedClick(source: DeviceModel): void;
    onStatisticsClick(source: DeviceModel): void
    lastChanged?: string;
}

const Device: React.SFC<DeviceProps> = (props) => {
    return <div className="tile">
        <div>
            {props.device.deviceType === "TOGGLE_DEVICE" && <ToggleDevice device={props.device} lastChanged={FormatDate(props.device.lastChanged)} onAutomatedClick={props.onAutomatedClick} onConfigClick={props.onConfigClick} onIconClick={props.onIconClick} onTimerClick={props.onTimerClick} onTimerAbortClick={props.onTimerAbortClick} onStatisticsClick={props.onStatisticsClick} />}
            {props.device.deviceType === "VALUE_DEVICE" && <ValueDevice device={props.device} lastChanged={FormatDate(props.device.lastChanged)} onAutomatedClick={props.onAutomatedClick} onConfigClick={props.onConfigClick} onIconClick={props.onIconClick} onTimerClick={props.onTimerClick} onTimerAbortClick={props.onTimerAbortClick} onStatisticsClick={props.onStatisticsClick} />}
        </div>
    </div>
}

export function FormatDate(unixTimestamp: number): string {
    var date = new Date(unixTimestamp * 1000);

    var hours : any = date.getHours();
    hours = ("0" + hours).slice(-2);

    var minutes: any = date.getMinutes();
    minutes = ("0" + minutes).slice(-2);

    var seconds: any = date.getSeconds();
    seconds = ("0" + seconds).slice(-2);

    return hours + ":" + minutes + ":" + seconds;
}

export default Device;

 

