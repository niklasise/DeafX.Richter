import * as React from "react";
import { DeviceProps } from "./Device"

const ValueDevice: React.SFC<DeviceProps> = (props) => {
    return <div>
            <div className="tileTopLeft">
                {props.device.isUpdating && <img src="dist/img/loader.svg" />}
                {!props.device.isUpdating && <i className="fa fa-thermometer-empty" />}
            </div>
            <div className="tileTopRight clickable" onClick={(e) => { e.stopPropagation(); props.onConfigClick(props.device) }}>
                <div className="fa fa-gear"></div>
            </div>
            <div className="tileCenter">
                <div>
                    <span>
                        {props.device.value}
                    </span>
                    <span className="valueUnit">&deg;</span>
                </div>
            </div>
            <div className="tileLabel">{props.device.title}</div>
            {props.device.isUpdating && <div className="loadingPanel"></div>}
        </div>
}

export default ValueDevice;