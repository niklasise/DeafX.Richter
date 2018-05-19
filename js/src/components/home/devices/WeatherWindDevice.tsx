import * as React from "react";
import { Device as DeviceModel, WeatherWindDevice as WeatherWindDeviceModel } from "models/device";
import Device from "./Device";
import styled from "styled-components";
import styles from "constants/styles";

export interface WeatherWindDeviceProps {
    device: WeatherWindDeviceModel,
    onStatisticsClick(source: DeviceModel): void
}

function renderTopRight(props: WeatherWindDeviceProps) : JSX.Element {
    return (
        <ClickableIcon 
            className="fa fa-bar-chart"
            onClick={(e) => { e.stopPropagation(); props.onStatisticsClick(props.device) }}
        />
    );
}

function renderBottomRight(props: WeatherWindDeviceProps) : JSX.Element {
    return (
            <WindIcon className="fa fa-location-arrow" rotation={props.device.direction-45} />
    );
}

function renderBottomLeft(props: WeatherWindDeviceProps) : JSX.Element {
    return (
            <div>
                {props.device.maxValue}
            </div>
    );
}

function renderCenter(props: WeatherWindDeviceProps) : JSX.Element {
    return (
        <div>
            <span>
                {props.device.value}
            </span>
            <UnitSpan>M/S</UnitSpan>
        </div>
    );
}

const WeatherWindDevice: React.SFC<WeatherWindDeviceProps> = (props) => {
    return (
        <Device 
            device={props.device}
            topRightContent={renderTopRight(props)}
            bottomRightContent={renderBottomRight(props)}
            bottomLeftContent={renderBottomLeft(props)}
            centerContent={renderCenter(props)}
        />
    )
}

const ClickableIcon = styled.i`
    cursor: pointer;
`

const WindIcon = styled.i`
    transform: rotate( ${(p: {rotation: number}) => p.rotation }deg);
`

const UnitSpan = styled.span`
    font-size: 30px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        font-size: 20px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        font-size: 16px;
    }
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

export default WeatherWindDevice;