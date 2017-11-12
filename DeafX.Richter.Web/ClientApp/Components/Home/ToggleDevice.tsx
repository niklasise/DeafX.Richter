import * as React from "react";
import { DeviceProps } from "./Device"
import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel } from "../../Models/Device";
import DeviceTimer from "./DeviceTimer";

const ToggleDevice: React.SFC<DeviceProps> = (props) => {
    return <div>
        <div className="tileTopLeft">
            {props.device.isUpdating && <img src="dist/img/loader.svg" />}
            {!props.device.isUpdating && <i className="fa fa-plug" />}
        </div>
        <div className="tileTopRight clickable" onClick={(e) => { e.stopPropagation(); props.onConfigClick(props.device) }}>
            <div className="fa fa-gear"></div>
        </div>
        <div className="tileCenter">
            <i className={"fa fa-lightbulb-o clickable" + ((props.device as ToggleDeviceModel).toggled ? "" : " off")} onClick={(e) => { props.onIconClick(props.device) }} />
        </div>
        <div className="tileBottomLeft">
            {props.device.deviceType === "TOGGLE_DEVICE" && <DeviceTimer device={props.device} onTimerClick={props.onTimerClick} timerValue={(props.device as ToggleDeviceModel).timerValue} />}
        </div>
        <div className="tileBottomRight">
            {props.device.deviceType === "TOGGLE_DEVICE" && <i className={(props.device as ToggleDeviceModel).automated ? "fa fa-refresh clickable" : "fa fa-refresh clickable off"} onClick={(e) => { e.stopPropagation(); props.onAutomatedClick(props.device) }} />}
        </div>
        <div className="tileLabel">{props.device.title}</div>

        {props.device.isUpdating && <div className="loadingPanel"></div>}
    </div>
}

export default ToggleDevice;
