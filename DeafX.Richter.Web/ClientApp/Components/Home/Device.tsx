import * as React from "react";
import { Link, } from "react-router-dom";
import { Device as DeviceModel } from "../../Models/Device";
import ToggleDevice from "./ToggleDevice";
import ValueDevice from "./ValueDevice";

export interface DeviceProps {
    device: DeviceModel;
    onIconClick(source: DeviceModel): void;
    onConfigClick(source: DeviceModel): void;
    onTimerClick(source: DeviceModel, timeLeft: number): void;
    onAutomatedClick(source: DeviceModel): void;
}

const Device: React.SFC<DeviceProps> = (props) => {
    return <div className="tile">
        <div>
            {props.device.deviceType === "TOGGLE_DEVICE" && <ToggleDevice device={props.device} onAutomatedClick={props.onAutomatedClick} onConfigClick={props.onConfigClick} onIconClick={props.onIconClick} onTimerClick={props.onTimerClick} />}
            {props.device.deviceType === "VALUE_DEVICE" && <ValueDevice device={props.device} onAutomatedClick={props.onAutomatedClick} onConfigClick={props.onConfigClick} onIconClick={props.onIconClick} onTimerClick={props.onTimerClick} />}
        </div>
    </div>
}

export default Device;

