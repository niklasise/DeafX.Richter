import * as React from "react";
import styled from "styled-components";
import RadioButton from "./RadioButton";

const WrapperDiv = styled.div`
    display: flex;
`;

const StyledRadioButton = styled(RadioButton) `
    flex: 1 1 0;
`;

export interface RadioButtonGroupProps {
    options: {
        name: string,
        value: string
    }[],
    selectedValue: string,
    onValueChanged: (value) => void,
    className?: string
}

const RadioButtonGroup: React.SFC<RadioButtonGroupProps> = (props) => {
    let onChanged = (value: string) => {
        if (value !== props.selectedValue)
        {
            props.onValueChanged(value);
        }
    };

    return <WrapperDiv className={props.className} >

        {props.options.map(function (option, index) {
            return <RadioButton key={option.value} text={option.name} onClicked={onChanged} value={option.value} />
        }, this)}

    </WrapperDiv>
}

export default RadioButtonGroup;