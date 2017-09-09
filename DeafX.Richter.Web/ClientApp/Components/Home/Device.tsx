import * as React from "react"
import { Device as DeviceModel } from "../../Models/Device"

export interface DeviceProps {
    device: DeviceModel;
    onClick(source:DeviceModel) : void;
}

export const Device: React.SFC<DeviceProps> = (props) => {
    return <div className="tile" onClick={(e) => { props.onClick(props.device) }}>
        <div>
            <div>
                <div className="tileTopRight">
                    <div className="fa fa-gear"></div>
                </div>
                <div className={"tileCenter" + (props.device.toggled ? "" : " off")}>
                    <div className="fa fa-lightbulb-o"></div>
                </div>
                <div className="tileBottomRight">
                    <div className="fa fa-refresh"></div>
                </div>
                <div className="tileLabel">{props.device.title}</div>
            </div>
        </div>
    </div>;
}

//Device.defaultProps = {
//    name: "Guest User", // This value is adopted when name prop is omitted.
//}
