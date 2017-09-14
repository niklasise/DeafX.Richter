import * as React from "react";
import { Link, } from "react-router-dom";
import { Device as DeviceModel, ToggleDevice, ValueDevice } from "../../Models/Device";

export interface DeviceProps {
    device: DeviceModel;
    onIconClick(source: DeviceModel): void;
    onConfigClick(source: DeviceModel): void;
}

export const Device: React.SFC<DeviceProps> = (props) => {
    return <div className="tile" onClick={(e) => { props.onIconClick(props.device) }}>
        <div>
            <div>
                <div className="tileTopLeft">
                    {props.device.deviceType === "VALUE_DEVICE" && <i className="fa fa-thermometer-empty" />}
                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-plug" />}
                </div>
                <div className="tileTopRight" onClick={(e) => { e.stopPropagation(); props.onConfigClick(props.device) }}>
                    <div className="fa fa-gear"></div>
                </div>
                <div className="tileCenter">

                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className={"fa fa-lightbulb-o" + ((props.device as ToggleDevice).toggled ? "" : " off")} />}
                    {props.device.deviceType === "VALUE_DEVICE" &&
                        <span style={{ position: "relative" }}>
                        {(props.device as ValueDevice).value}
                        <span style={{ position: "absolute", top: 13, right: -30, fontSize: 80 }}>&deg;</span>
                        </span>}
                </div>
                <div className="tileBottomLeft">
                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-clock-o off" />}
                </div>
                <div className="tileBottomRight">
                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-refresh" />}
                </div>
                <div className="tileLabel">{props.device.title}</div>
            </div>
        </div>
    </div>;
}

