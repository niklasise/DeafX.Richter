import * as React from "react";
import { BorderRadius, ContainerOpacity, FontColor, Padding, PrimaryColor } from "../Constants/Styles"
import styled from "styled-components";

export interface StatisticsBadgeProps {
    label: string,
    value: string
    className?: string
}

const ContainerDiv = styled.div`
    border-radius: ${BorderRadius};
    background-color: ${PrimaryColor};
    color: ${FontColor};
    padding: ${Padding};
    opacity: ${ContainerOpacity};
`
const LeftSpan = styled.div`
    float: left;
`
const RightSpan = styled.div`
    float: right;
`

const OverflowDiv = styled.div`
    overflow: hidden;
`


const StatisticsBadge: React.SFC<StatisticsBadgeProps> = (props) => {
    return <ContainerDiv className={props.className} >
        <OverflowDiv>
            <LeftSpan>{props.label}</LeftSpan>
            <RightSpan>{props.value}</RightSpan>
        </OverflowDiv>
    </ContainerDiv>
}

export default StatisticsBadge;