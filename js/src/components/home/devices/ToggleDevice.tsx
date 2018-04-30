import * as React from "react";
import { Device as DeviceModel, ToggleDevice as ToggleDeviceModel } from "models/device";
import DeviceTimer from "./DeviceTimer";
import Device from "./Device";
import styled from "styled-components";
import * as classnames from "classnames";
//import styles from "constants/styles";

const ClickableIcon = styled.i`
    cursor: pointer;

    &.toggle-off {
        opacity: 0.3;
    }
`;

export interface ToggleDeviceProps {
    device: DeviceModel,
    onIconClick(source: DeviceModel): void;
    onTimerClick(source: DeviceModel): void;
    onTimerAbortClick(source: DeviceModel): void;
    onAutomatedClick(source: DeviceModel): void; 
}

function renderCenter(props: ToggleDeviceProps) : JSX.Element {
    let className = classnames("fa fa-lightbulb-o", {"off": (props.device as ToggleDeviceModel).value})
    
    return (
        <ClickableIcon 
            className={className} 
            onClick={(e) => { props.onIconClick(props.device) }} 
        />
    )
}

function renderBottomLeft(props: ToggleDeviceProps) : JSX.Element {  
    return (
        <DeviceTimer 
            device={props.device as ToggleDeviceModel} 
            onTimerClick={props.onTimerClick} 
            onTimerAbortClick={props.onTimerAbortClick} 
        />
    )
}

function renderBottomRight(props: ToggleDeviceProps) : JSX.Element {  
    let className = classnames("fa fa-refresh", {"toggle-off": (props.device as ToggleDeviceModel).automated})

    return (
        <ClickableIcon
            className={className} 
            onClick={(e) => { e.stopPropagation(); props.onAutomatedClick(props.device) }} 
        />
    )
}

const ToggleDevice: React.SFC<ToggleDeviceProps> = (props) => {

    
    return (
        <Device
            device={props.device}
            centerContent={renderCenter(props)}
            bottomLeftContent={renderBottomLeft(props)}
            bottomRightContent={renderBottomRight(props)}
        />
    )
}

export default ToggleDevice;
