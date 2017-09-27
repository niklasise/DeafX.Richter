import * as React from "react";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";

export interface DeviceClockProps {
    setTimerActive: boolean;
    onTimerClick(): void;
}

const DeviceTimer: React.SFC<DeviceClockProps> = (props) => {
    return <div>
        {!props.setTimerActive && <i className="fa fa-clock-o" onClick={(e) => { e.stopPropagation(); props.onTimerClick() } } />}
        {props.setTimerActive &&
            <div className="timerInput">
                01:33
                <i className="fa fa-caret-up" />
                <i className="fa fa-caret-down" />
            </div>
        }
    </div>;
}

export default DeviceTimer;

