import * as React from "react";
import styled from "styled-components";
import { LightColor } from "../../Constants/Styles";

const StyledButton = styled.button`
    height: 42px;
    font-size: 14px;
    border-radius: 5px;
    border: 0;
    text-align: center;
    color: #FFF;
    background-color:  ${LightColor};
    cursor: pointer;
    padding: 0 20px;

    img {
        margin-top: 5px;
        height: 18px;
    }
`;

export interface RadioButtonProps {
    text: string,
    value: string,
    onClicked: (value: string) => void,
    className?: string,
    loading?: boolean
}

const RadioButton: React.SFC<RadioButtonProps> = (props) => {
    return <button className={props.className} onClick={(e) => { props.onClicked(props.value) }}>
        {props.loading && <img src="dist/img/loader.svg" />}
        {!props.loading && <span>{props.text}</span>}
    </button>
}

export default RadioButton;
