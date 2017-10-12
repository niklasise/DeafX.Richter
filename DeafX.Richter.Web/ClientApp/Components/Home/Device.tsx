import * as React from "react";
import { Link, } from "react-router-dom";
import { Device as DeviceModel, ToggleDevice, ValueDevice } from "../../Models/Device";
import DeviceTimer from "./DeviceTimer";

export interface DeviceProps {
    device: DeviceModel;
    onIconClick(source: DeviceModel): void;
    onConfigClick(source: DeviceModel): void;
    onTimerClick(source: DeviceModel): void;
}

export interface DeviceState {
    //setTimerActive: boolean;
}

//export const Device: React.SFC<DeviceProps> = (props) => {
//    return <div className="tile" onClick={(e) => { props.onIconClick(props.device) }}>
//        <div>
//            <div>
//                <div className="tileTopLeft">
//                    {props.device.isUpdating && <img src="dist/img/loader.svg" />}
//                    {!props.device.isUpdating && props.device.deviceType === "VALUE_DEVICE" && <i className="fa fa-thermometer-empty" />}
//                    {!props.device.isUpdating && props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-plug" />}
//                </div>
//                <div className="tileTopRight" onClick={(e) => { e.stopPropagation(); props.onConfigClick(props.device) }}>
//                    <div className="fa fa-gear"></div>
//                </div>
//                <div className="tileCenter">

//                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className={"fa fa-lightbulb-o" + ((props.device as ToggleDevice).toggled ? "" : " off")} />}
//                    {props.device.deviceType === "VALUE_DEVICE" &&
//                        <span style={{ position: "relative" }}>
//                        {(props.device as ValueDevice).value}
//                        <span style={{ position: "absolute", top: 13, right: -30, fontSize: 80 }}>&deg;</span>
//                        </span>}
//                </div>
//                <div className="tileBottomLeft">
//                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-clock-o" />}
//                </div>
//                <div className="tileBottomRight">
//                    {props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-refresh" />}
//                </div>
//                <div className="tileLabel">{props.device.title}</div>

//                {props.device.isUpdating && <div className="loadingPanel"></div>}
//            </div>
//        </div>
//    </div>;
//}

export class Device extends React.Component<DeviceProps, DeviceState> {

    constructor() {
        super();
        //this.onTimerClick = this.onTimerClick.bind(this);

        //this.state = {
        //    setTimerActive: false
        //};
    }

    //onTimerClick(): void {
    //    this.setState({ ...this.state, setTimerActive: true })
    //}
    
    public render() {
        return <div className="tile">
                    <div>
                        <div>
                            <div className="tileTopLeft">
                                {this.props.device.isUpdating && <img src="dist/img/loader.svg" />}
                                {!this.props.device.isUpdating && this.props.device.deviceType === "VALUE_DEVICE" && <i className="fa fa-thermometer-empty" />}
                                {!this.props.device.isUpdating && this.props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-plug" />}
                            </div>
                            <div className="tileTopRight" onClick={(e) => { e.stopPropagation(); this.props.onConfigClick(this.props.device) }}>
                                <div className="fa fa-gear"></div>
                            </div>
                            <div className="tileCenter">

                                {this.props.device.deviceType === "TOGGLE_DEVICE" && <i className={"fa fa-lightbulb-o" + ((this.props.device as ToggleDevice).toggled ? "" : " off")} onClick={(e) => { this.props.onIconClick(this.props.device) }}/>}
                                {this.props.device.deviceType === "VALUE_DEVICE" &&
                                    <span style={{ position: "relative" }}>
                                    {(this.props.device as ValueDevice).value}
                                    <span style={{ position: "absolute", top: 13, right: -30, fontSize: 80 }}>&deg;</span>
                                    </span>}
                            </div>
                            <div className="tileBottomLeft">
                                {this.props.device.deviceType === "TOGGLE_DEVICE" && <DeviceTimer device={this.props.device} onTimerClick={this.props.onTimerClick} timerValue={(this.props.device as ToggleDevice).timerValue} />}
                            </div>
                            <div className="tileBottomRight">
                                {this.props.device.deviceType === "TOGGLE_DEVICE" && <i className="fa fa-refresh" />}
                            </div>
                            <div className="tileLabel">{this.props.device.title}</div>

                            {this.props.device.isUpdating && <div className="loadingPanel"></div>}
                        </div>
                    </div>
                </div>;
    }
}

