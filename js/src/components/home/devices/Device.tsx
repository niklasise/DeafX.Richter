import * as React from "react";
import { Link, } from "react-router-dom";
import { Device as DeviceModel } from "models/device";
import ToggleDevice from "./ToggleDevice";
import ValueDevice from "./ValueDevice";
import styled from "styled-components";
import styles from "constants/styles";

export interface DeviceProps {
    device: DeviceModel,
    lastChanged?: string,
    topLeftContent?: object,
    topRightContent?: object,
    centerContent?: object,
    bottomLeftContent?: object,
    bottomRightContent?: object,
    
}

const Device: React.SFC<DeviceProps> = (props) => {
    return (
        <TileDiv>
            <div>
                <div>
                    <TopLeftDiv>
                        {props.device.isUpdating && <img src="dist/img/loader.svg" />}
                        {!props.device.isUpdating && props.topLeftContent}
                    </TopLeftDiv>
                    <TopRightDiv>
                        {props.topRightContent}
                    </TopRightDiv>
                    <TopCenterDiv>
                        {props.lastChanged}
                    </TopCenterDiv>
                    <CenterDiv>
                        {props.centerContent}
                    </CenterDiv>
                    <BottomLeftDiv>
                        {props.bottomLeftContent}
                    </BottomLeftDiv>
                    <BottomRightDiv>
                        {props.bottomRightContent}
                    </BottomRightDiv>
                    <LabelDiv>
                        {props.device.title}
                    </LabelDiv>
                    {props.device.isUpdating && <div className="loadingPanel"></div>}
                </div>
            </div>
        </TileDiv>
    );
}

const TileDiv = styled.div`
    position: relative;
    float: left;
    width: 50%;

    &:after {
        content: "";
        display: block;
        padding-bottom: 100%;
    }

    > div {
        position: absolute;
        height: 100%;
        width: 100%;
        padding: 0 20px 20px 0;

        @media screen and (max-width: ${styles.breakpoints.large}) {
            padding: 0 15px 15px 0;
        }

        @media screen and (max-width: ${styles.breakpoints.medium}) {
            padding: 0 10px 10px 0;
        }

        > div {
            background-color: ${styles.colors.primary};
            color: ${styles.colors.fontPrimary};
            text-align: center;
            height: 100%;
            width: 100%;
            display: table;
            position: relative;
            border-radius: 5px;
            opacity: 0.7;

            .loadingPanel {
                width: 100%;
                height: 100%;
                position: absolute;
                top: 0;
                left: 0;
                background: #000000;
                opacity: 0.2;
                border-radius: 5px;
            } 
        }
    }

    &.large {
        width: 100%;

        &:after {
            padding-bottom: 50%;
        }
    }
`

const CornerDiv = styled.div`
    position: absolute;

    * {
        vertical-align: middle;
    }

    img {
        width: 30px;

        @media screen and (max-width: ${styles.breakpoints.large}) {
            width: 25px;
        }

        @media screen and (max-width: ${styles.breakpoints.medium}) {
            width: 20px;
        }
    }

    i {
        font-size: 30px;

        @media screen and (max-width: ${styles.breakpoints.large}) {
            font-size: 25px;
        }

        @media screen and (max-width: ${styles.breakpoints.medium}) {
            font-size: 20px;
        }
    }

    span {
        margin-left: 10px;
        margin-right: 10px;
        font-size: 16px;

        @media screen and (max-width: ${styles.breakpoints.large}) {
            font-size: 13px;
            margin-left: 5px;
            margin-right: 5px;
        }

        @media screen and (max-width: ${styles.breakpoints.medium}) {
            font-size: 11px;
        }
    }
`

const LeftDiv = CornerDiv.extend`
    left: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        left: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        left: 10px;
    }
`
const TopLeftDiv = LeftDiv.extend`
    top: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        top: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        top: 10px;
    }
`;

const BottomLeftDiv = LeftDiv.extend`
    bottom: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        bottom: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        bottom: 10px;
    }
`

const RightDiv = CornerDiv.extend`
    right: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        right: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        right: 10px;
    }
`

const TopRightDiv = RightDiv.extend`
    top: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        top: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        top: 10px;
    }
`

const BottomRightDiv = RightDiv.extend`
    bottom: 20px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        bottom: 15px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        bottom: 10px;
    }
`

const TopCenterDiv = styled.div`
    position: absolute;
    top: 20px;
    left: 0;
    right: 0;
    line-height: 30px;
    font-size: 12px;
    opacity: 0.7;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        top: 15px;
        line-height: 25px;
        font-size: 10px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        top: 10px;
        line-height: 15px;
    }
`

const CenterDiv  = styled.div`
    display: table-cell;
    vertical-align: middle;

    font-size: 150px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        font-size: 110px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        font-size: 80px;
    }

    @media screen and (max-width: ${styles.breakpoints.small}) {
        font-size: 60px;
    }
`

const LabelDiv = styled.div`
    position: absolute;
    width: 100%;
    text-align: center;
    bottom: 60px;
    left: 0;
    font-size: 18px;

    @media screen and (max-width: ${styles.breakpoints.large}) {
        font-size: 16px;
        bottom: 40px;
    }

    @media screen and (max-width: ${styles.breakpoints.medium}) {
        font-size: 14px;
        bottom: 30px;
    }

    @media screen and (max-width: ${styles.breakpoints.small}) {
        font-size: 12px;
    }
`

export function FormatDate(unixTimestamp: number): string {
    var date = new Date(unixTimestamp * 1000);

    var hours : any = date.getHours();
    hours = ("0" + hours).slice(-2);

    var minutes: any = date.getMinutes();
    minutes = ("0" + minutes).slice(-2);

    var seconds: any = date.getSeconds();
    seconds = ("0" + seconds).slice(-2);

    return hours + ":" + minutes + ":" + seconds;
}

export default Device;

 

