import * as React from "react";
import { Device as DeviceModel, SunDevice as SunDeviceModel } from "models/device";
import Device from "./Device";
import styled from "styled-components";
import styles from "constants/styles";

export interface SunDeviceProps {
    device: SunDeviceModel
}

function renderBottomLeft(props: SunDeviceProps) : JSX.Element {
    return (
        <>
            <i className="wi wi-sunrise"/>
            <span>{FormatDate(props.device.sunRise)}</span>       
        </>
    );
}

function renderBottomRight(props: SunDeviceProps) : JSX.Element {
    return (
        <>
            <span>{FormatDate(props.device.sunSet)}</span>       
            <i className="wi wi-sunset"/>
        </>
    );
}

function renderCenter(props: SunDeviceProps) : JSX.Element {
    return (
        <i className="wi wi-day-sunny"/>
    );
}

function FormatDate(unixTimestamp: number): string {
    var date = new Date(unixTimestamp * 1000);

    var hours : any = date.getHours();
    hours = ("0" + hours).slice(-2);

    var minutes: any = date.getMinutes();
    minutes = ("0" + minutes).slice(-2);

    return hours + ":" + minutes;
}

const SunDevice: React.SFC<SunDeviceProps> = (props) => {
    return (
        <Device 
            device={props.device}
            bottomLeftContent={renderBottomLeft(props)}
            bottomRightContent={renderBottomRight(props)}
            centerContent={renderCenter(props)}
        />
    )
}

const ClickableIcon = styled.i`
    cursor: pointer;
`

export default SunDevice;