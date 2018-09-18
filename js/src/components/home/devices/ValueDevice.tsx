import * as React from "react";
import { Device as DeviceModel } from "models/device";
import Device from "./Device";
import styled from "styled-components";
import styles from "constants/styles";

export interface ValueDeviceProps {
    device: DeviceModel,
    onStatisticsClick(source: DeviceModel): void
}

function renderTopRight(props: ValueDeviceProps) : JSX.Element {
    return (
        <ClickableIcon 
            className="fa fa-bar-chart"
            onClick={(e) => { e.stopPropagation(); props.onStatisticsClick(props.device) }}
        />
    );
}

function renderCenter(props: ValueDeviceProps) : JSX.Element {
    return (
        <div>
            <span>
                {props.device.value}
            </span>
            <span>{getValueUnit(props)}</span>
        </div>
    );
}

function getValueUnit(props: ValueDeviceProps) : JSX.Element {
    
    switch(props.device.valueType) {
        case "Temperature":
            return <>&deg;</>
        default:
            return null;
    }
}

const ValueDevice: React.SFC<ValueDeviceProps> = (props) => {
    return (
        <Device 
            device={props.device}
            topRightContent={renderTopRight(props)}
            centerContent={renderCenter(props)}
        />
    )
}

const ClickableIcon = styled.i`
    cursor: pointer;
`

export default ValueDevice;