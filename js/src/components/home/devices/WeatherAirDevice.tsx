import * as React from "react";
import { Device as DeviceModel, WeatherAirDevice as WeatherAirDeviceModel } from "models/device";
import Device from "./Device";
import styled from "styled-components";
import styles from "constants/styles";

export interface WeatherAirDeviceProps {
    device: WeatherAirDeviceModel,
    onStatisticsClick(source: DeviceModel): void
}

function renderTopRight(props: WeatherAirDeviceProps) : JSX.Element {
    return (
        <ClickableIcon 
            className="fa fa-bar-chart"
            onClick={(e) => { e.stopPropagation(); props.onStatisticsClick(props.device) }}
        />
    );
}

function renderBottomLeft(props: WeatherAirDeviceProps) : JSX.Element {
    return (
        <>
            <i className="fa fa-tint"/>
            <span>{props.device.relativeHumidity}%</span>       
        </>
    );
}

function renderCenter(props: WeatherAirDeviceProps) : JSX.Element {
    return (
        <div>
            <span>
                {props.device.value}
            </span>
            <span>&deg;</span>
        </div>
    );
}

const WeatherAirDevice: React.SFC<WeatherAirDeviceProps> = (props) => {
    return (
        <Device 
            device={props.device}
            topRightContent={renderTopRight(props)}
            bottomLeftContent={renderBottomLeft(props)}
            centerContent={renderCenter(props)}
        />
    )
}

const ClickableIcon = styled.i`
    cursor: pointer;
`

export default WeatherAirDevice;