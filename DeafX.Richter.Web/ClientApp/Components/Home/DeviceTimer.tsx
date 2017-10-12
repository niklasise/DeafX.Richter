import * as React from "react";
import { Device as DeviceModel, ToggleDevice } from "../../Models/Device";

export interface DeviceClockProps {
    device: DeviceModel;
    timerValue: number;
    onTimerClick(source: DeviceModel): void;
}

const DeviceTimer: React.SFC<DeviceClockProps> = (props) => {
    function formatTimerValue(s: number): string
    {
        return (s - (s %= 60)) / 60 + (9 < s ? ':' : ':0') + s; 
    }

    return <div>
        {!!props.timerValue &&
            <div>{formatTimerValue(props.timerValue)}</div>
        }
        {!props.timerValue && <i className="fa fa-clock-o" onClick={(e) => { e.stopPropagation(); props.onTimerClick(props.device); }} />}

    </div>;
}

export default DeviceTimer;

//{props.setTimerActive && !props.timerValue &&
//    <div className="timerInput">
//        01:33
//        <div className="carets">
//            <i className="fa fa-caret-up" />
//            <i className="fa fa-caret-down" />
//        </div>
//        <i className="fa fa-check" />
//        <i className="fa fa-times" />
//    </div>
//}