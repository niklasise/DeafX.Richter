import * as React from "react";
import { Device as DeviceModel } from "models/device";
import Device from "./Device";
import styled from "styled-components";
import styles from "constants/styles";

export interface ValueDeviceProps {
    device: DeviceModel,
    onStatisticsClick(source: DeviceModel): void
}

const UnitDiv = styled.div`
    position: absolute;
    margin-left: -10px;

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        margin-left: -5px;
    }
`

function renderTopRight(props: ValueDeviceProps) : JSX.Element {
    return (
        <i 
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
            <span>&deg;</span>
        </div>
    );
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

export default ValueDevice;