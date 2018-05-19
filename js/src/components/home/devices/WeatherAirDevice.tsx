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

function renderTopLeft(props: WeatherAirDeviceProps) : JSX.Element {
    return (
        <div>
            <i className="fa fa-tint"/>
            <IconTextSpan>{props.device.relativeHumidity}%</IconTextSpan>       
        </div>
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
            bottomLeftContent={renderTopLeft(props)}
            centerContent={renderCenter(props)}
        />
    )
}

const ClickableIcon = styled.i`
    cursor: pointer;
`

const IconTextSpan = styled.span`
    font-size: 15px;
    font-weight: bold;
    margin-left: 10px;
    position: relative;
    bottom: 6px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        font-size: 13px;
        bottom: 5px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        font-size: 10px;
        bottom: 2px;
        margin-left: 5px;
    }
`

export default WeatherAirDevice;