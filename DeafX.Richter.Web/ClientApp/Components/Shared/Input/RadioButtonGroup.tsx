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
        value: any
    }[],
    selectedValue: any,
    onValueChanged: (value: any) => void,
    className?: string,
    loading?: boolean
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
            let selected = props.selectedValue === option.value;

            return <StyledRadioButton key={option.value} text={option.name} onClicked={onChanged} value={option.value} selected={selected} loading={selected && props.loading} />
        }, this)}

    </WrapperDiv>
}

export default RadioButtonGroup;