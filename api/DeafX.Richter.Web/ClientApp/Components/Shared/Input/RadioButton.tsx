import * as React from "react";
import styled from "styled-components";
import { LightColor } from "../../Constants/Styles";
import classNames from "classnames";

const StyledButton = styled.button`
    height: 42px;
    font-size: 14px;
    border: 3px solid ${LightColor};
    border-right-width: 0;
    text-align: center;
    color: #FFF;
    background-color: unset;
    cursor: pointer;
    padding: 0 20px;
    outline: 0;
    margin: 0;

    img {
        margin-top: 5px;
        height: 18px;
    }

    &.selected {
        background-color: ${LightColor}
    }

    :first-of-type {
        border-top-left-radius: 5px;
        border-bottom-left-radius: 5px;
        
    }

    :last-of-type {
        border-top-right-radius: 5px;
        border-bottom-right-radius: 5px;
        border-right-width: 2px;
    }
`;

export interface RadioButtonProps {
    text: string,
    value: any,
    onClicked: (value: any) => void,
    selected: boolean,
    className?: string,
    loading?: boolean
}

const RadioButton: React.SFC<RadioButtonProps> = (props) => {
    return <StyledButton className={classNames(props.className, { "selected": props.selected })} onClick={(e) => { props.onClicked(props.value) }}>
        {props.loading && <img src="/dist/img/loader.svg" />}
        {!props.loading && <span>{props.text}</span>}
    </StyledButton>
}

export default RadioButton;
