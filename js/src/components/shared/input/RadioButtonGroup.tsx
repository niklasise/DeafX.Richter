import * as React from "react";
import styled from "styled-components";
import RadioButton from "./RadioButton";
import { LightColor } from "constants/styles";

const WrapperDiv = styled.div`
    display: flex;
    position: relative;
`;

const DisabledOverlayDiv = styled.div`
    position: absolute;
    background: ${LightColor};
    opacity: 0.4;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    border-radius: 5px;
`;

const StyledRadioButton = styled(RadioButton) `
    flex: 1 1 0;
`;

export interface RadioButtonGroupProps {
    options: {
        name: string,
        value: any
    }[],
    selectedValue: any,
    onValueChanged: (value: any) => void,
    className?: string,
    loading?: boolean,
    disabled: boolean,
}

const RadioButtonGroup: React.SFC<RadioButtonGroupProps> = (props) => {
    let onChanged = (value: any) => {
        if (!props.disabled && value !== props.selectedValue)
        {
            props.onValueChanged(value);
        }
    };

    return <WrapperDiv className={props.className} >

        {props.options.map(function (option, index) {
            let selected = props.selectedValue === option.value;

            return <StyledRadioButton key={option.value} text={option.name} onClicked={onChanged} value={option.value} selected={selected} loading={selected && props.loading} />
        }, this)}

        {props.disabled && <DisabledOverlayDiv/>}

    </WrapperDiv>
}

export default RadioButtonGroup;